using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using FlmAutoRent.Presentation.Areas.Admin.Models;
using FlmAutoRent.Data.Entities;
using FlmAutoRent.Services;
using Microsoft.AspNetCore.DataProtection;

namespace FlmAutoRent.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OperatorController : Controller
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "cxz92k13md8f981hu6y7alkc";
        private readonly IOperatorServices _operatorServices;
        private readonly IEmailServices _emailServices;

        public OperatorController(IDataProtectionProvider dataProtectionProvider, IOperatorServices operatorService, IEmailServices emailServices){
            this._dataProtectionProvider = dataProtectionProvider;
            this._operatorServices = operatorService;
            this._emailServices = emailServices;
        }

        public IActionResult Index(){
            ViewBag.HiddenMenuAside = true;

            var model = new OperatorAddViewModel();
            var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
            var profilingOperator = _operatorServices.GetOperatorByUserId(UserId);
            
            model.Id = profilingOperator.Id;
            model.UserId = profilingOperator.UserId;
            model.Name = profilingOperator.Name;
            model.LastName = profilingOperator.Lastname;
            model.EmailAddress = profilingOperator.Email;
            model.PhoneNr = profilingOperator.PhoneNr;
            model.PhoneNr = profilingOperator.PhoneNr;
            model.EnabledId = profilingOperator.Enabled;
            model.GroupId = profilingOperator.ProfilingOperatorGroups.FirstOrDefault().Groups.Id;
            model.Password = profilingOperator.Password;
            model.PasswordDeadLineValue = profilingOperator.PasswordDeadline;

            model.Enabled = new List<SelectListItem>{ 
                new SelectListItem { Value="", Text = "Seleziona un valore ..." },
                new SelectListItem { Value="0", Text = "Disabilitato" },
                new SelectListItem { Value="1", Text = "Abilitato" }
            };
            
            model.PasswordDeadline = new List<SelectListItem>{
                new SelectListItem{ Value= "120", Text="Scandenza a 120 giorni" },
                new SelectListItem{ Value= "60", Text="Scandenza a 60 giorni" },
                new SelectListItem{ Value= "30", Text="Scandenza a 30 giorni" },
                new SelectListItem{ Value= "0", Text="Nessuna scandenza" },
            };

            model.GroupList =  new[] { new SelectListItem { Value="", Text = "Seleziona un gruppo ..." } }.Concat(_operatorServices.GetListGroup().Select(x => new SelectListItem{
                Value = x.Id.ToString(),
                Text = x.Name
            }));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(OperatorAddViewModel model, int Id){
           var entitiesOperator = new ProfilingOperator();

            if(ModelState.IsValid){
                entitiesOperator.Guid = Guid.Empty;
                entitiesOperator.Name = model.Name;
                entitiesOperator.Lastname = model.LastName;
                entitiesOperator.UserId = model.UserId;
                entitiesOperator.Email = model.EmailAddress;
                entitiesOperator.PhoneNr = model.PhoneNr;
                entitiesOperator.PasswordDeadline = model.PasswordDeadLineValue;
                entitiesOperator.OperatorData = DateTime.Now;
                entitiesOperator.PasswordLastEdit = DateTime.Now;
            }

            if(_operatorServices.ExistOperator(model.UserId, model.EmailAddress)){
                entitiesOperator.Id = Id;
                _operatorServices.UpdateOperator(entitiesOperator);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ChangePassword(){
            var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
            var profilingOperator = _operatorServices.GetOperatorByUserId(UserId);

            var operatorGuid = Guid.NewGuid();
            _operatorServices.ResetProfilingOperatorPassword(profilingOperator, operatorGuid);

            //SEND EMAIL ACTIVE ACCOUNT
            var EmailSender = new EmailSenderViewModel();
            EmailSender.EmailTemplate = Path.GetFullPath("EmailTemplate/EmailTemplate.html");
            EmailSender.EmailObject = "FLM Auto & Rent | Aggiorna Password";
            EmailSender.EmailTo = profilingOperator.Email;
            EmailSender.EmailBody = String.Concat("Ciao ", profilingOperator.Name, " ", profilingOperator.Lastname, " (UserID: ", profilingOperator.UserId, ")", "<br /> Ti invitiamo a collegarti al seguente link:<br /><a target='_blank' style='padding: 0; margin: 0; width: auto; height: auto; background: none trasparent; border: 0px none trasparent; outline: 0px none; font-size: 18px; line-height: 36px; text-align: left; font-weight: 400; text-decoration: none; color: #1E88E5; vertical-align: top;' href='", this.Request.Scheme, "://", this.Request.Host, this.Request.PathBase,  "/Admin/Account/Password/",operatorGuid, "'>",   this.Request.Scheme, "://", this.Request.Host, this.Request.PathBase,  "/Admin/Account/Password/",operatorGuid, "</a><br /> Per aggiornare la password di accesso al Gestionale FLM Auto & Rent<br /><br /> Lo Staff");

            _emailServices.EmailSender(EmailSender.EmailTemplate, EmailSender.EmailObject, EmailSender.EmailTo, EmailSender.EmailBody);    
            
            return RedirectToAction("Logout", "Account");
        }
    }
}