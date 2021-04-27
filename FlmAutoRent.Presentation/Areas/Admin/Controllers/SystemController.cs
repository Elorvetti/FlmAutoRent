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
    public class SystemController : Controller
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "cxz92k13md8f981hu6y7alkc";
        private readonly IProfilingGroupServices _profilingGroupServices;
        private readonly IEmailServices _emailServices;
        private readonly IOperatorServices _operatorServices;
        private readonly ILogServices _logService;
        
        public SystemController(IDataProtectionProvider dataProtectionProvider, IProfilingGroupServices profilingGroupServices, IEmailServices emailServices, IOperatorServices operatorServices, ILogServices logServices){
            this._dataProtectionProvider = dataProtectionProvider;
            this._profilingGroupServices = profilingGroupServices;
            this._emailServices = emailServices;
            this._operatorServices = operatorServices;
            this._logService = logServices;
        }
        
        public IActionResult Index(){
            return RedirectToAction("Groups");
        }
        
        public IActionResult Groups(int pageSize = 10, int pageNumber = 1){
            ViewData["Title"] = "Gruppi";
            var model = new GroupsTableViewModel();

            model.HowManyField = pageSize;
            model.HowManyFieldList = new List<GroupsTableViewModel.HowManyFields>{
                new GroupsTableViewModel.HowManyFields{ Value = 10, DisplayText="Visualizza 10 risultati per pagina" },
                new GroupsTableViewModel.HowManyFields{ Value = 100, DisplayText="Visualizza 100 risultati per pagina" },
                new GroupsTableViewModel.HowManyFields{ Value = 1000, DisplayText="Visualizza 1000 risultati per pagina" },
                new GroupsTableViewModel.HowManyFields{ Value = 100000, DisplayText="Visualizza tutti i risultati" }
            };

            //Pagination
            model.totalRecords = _profilingGroupServices.GetProfilingGroups().Count;
            model.pageNumber = pageNumber;
            model.pageSize = pageSize;
            model.pageTotal =  Math.Ceiling((double)model.totalRecords / pageSize);
            var excludeRecords = (pageSize * pageNumber) - pageSize;  
            model.displayRecord = model.pageSize * pageNumber;
            if(model.displayRecord > model.pageTotal){
                model.displayRecord = model.totalRecords;
            }

            if(model.totalRecords > pageSize){
                model.displayPagination = true;
            }

            var ProfilingGroups = _profilingGroupServices.GetProfilingGroups(excludeRecords, pageSize);
            model.GroupsListViewModel = new List<GroupsViewModel>();
            
            foreach(var ProfilingGroup in ProfilingGroups){
                model.GroupsListViewModel.Add(new GroupsViewModel{ 
                    Id = ProfilingGroup.Id, 
                    Name = ProfilingGroup.Name,
                    Date = ProfilingGroup.Data, 
                    OperatorsInThisGroup = ProfilingGroup.ProfilingOperatorGroups.Where(x => x.Groups.Id == ProfilingGroup.Id).Count() 
                });        
            }
            
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Groups(GroupsTableViewModel model, int pageNumber = 1){
            ViewData["Title"] = "Gruppi";

            model.HowManyFieldList = new List<GroupsTableViewModel.HowManyFields>{
                new GroupsTableViewModel.HowManyFields{ Value = 10, DisplayText="Visualizza 10 risultati per pagina" },
                new GroupsTableViewModel.HowManyFields{ Value = 100, DisplayText="Visualizza 100 risultati per pagina" },
                new GroupsTableViewModel.HowManyFields{ Value = 1000, DisplayText="Visualizza 1000 risultati per pagina" },
                new GroupsTableViewModel.HowManyFields{ Value = 100000, DisplayText="Visualizza tutti i risultati" }
            };

            //Pagination
            model.totalRecords = _profilingGroupServices.GetProfilingGroupsByName(model.Find).Count;
            model.pageNumber = pageNumber;
            model.pageSize = model.HowManyField;
            model.pageTotal =  Math.Ceiling((double)model.totalRecords / model.HowManyField);
            var excludeRecords = (model.HowManyField * pageNumber) - model.HowManyField;  
            model.displayRecord = model.pageSize * pageNumber;
            if(model.displayRecord > model.pageTotal){
                model.displayRecord = model.totalRecords;
            }

            if(model.totalRecords > model.HowManyField){
                model.displayPagination = true;
            }
            
            var ProfilingGroups = _profilingGroupServices.GetProfilingGroupsByName(model.Find, excludeRecords, model.pageSize);
            model.GroupsListViewModel = new List<GroupsViewModel>();
            
            foreach(var ProfilingGroup in ProfilingGroups){
                model.GroupsListViewModel.Add(new GroupsViewModel{ 
                    Id = ProfilingGroup.Id, 
                    Name = ProfilingGroup.Name,
                    Date = ProfilingGroup.Data, 
                    OperatorsInThisGroup = ProfilingGroup.ProfilingOperatorGroups.Where(x => x.Groups.Id == ProfilingGroup.Id).Count() 
                });        
            }
            
            
            return View(model);
        }

        public IActionResult GroupsAddOrEdit(int Id){
            ViewData["Title"] = "Aggiungi Gruppo";
            var model = new GroupAddViewModel();
            
            if(Id != 0){
                var ProfilingGroup = _profilingGroupServices.GetProfilingGroupById(Id);
                model.Id = ProfilingGroup.Id;
                model.Name = ProfilingGroup.Name;

                var menusList = _profilingGroupServices.GetSystemMenus();
                foreach(var menuList in menusList){
                    var ActiveMenu = false;
                    if(ProfilingGroup.ProfilingGroupSystemMenus.Where( x => x.SystemMenus.Id == menuList.Id).Count() > 0){
                        ActiveMenu = true;
                    }

                    model.SystemMenuList.Add(new SystemMenu(){ Id = menuList.Id, MenuFatherId = menuList.MenuFatherId, Name = menuList.Name, Active = ActiveMenu });
                }

                var Operators = _operatorServices.GetOperators();
                foreach(var Operator in Operators){
                    var ActiveOperator = false;
                    if(ProfilingGroup.ProfilingOperatorGroups.Where(x => x.Operators.Id == Operator.Id).Any()){
                        ActiveOperator = true;
                    }

                    model.OperatorsList.Add(new OperatorList{ Id = Operator.Id, Email = Operator.Email, Name = Operator.Name, Lastname = Operator.Lastname, Active = ActiveOperator });
                }
            } else {
     
                var menusList = _profilingGroupServices.GetSystemMenus();
                foreach(var menuList in menusList){
                    model.SystemMenuList.Add(new SystemMenu(){ Id = menuList.Id, MenuFatherId = menuList.MenuFatherId, Name = menuList.Name, Active = false });
                }
                
       
             var Operators = _operatorServices.GetOperators();
                foreach(var Operator in Operators){
                    model.OperatorsList.Add(new OperatorList{ Id = Operator.Id, Email = Operator.Email, Name = Operator.Name, Lastname = Operator.Lastname, Active = false });
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GroupsAddOrEdit(GroupAddViewModel model, int Id){
            var entitiesGroup = new ProfilingGroup();
            
            if(ModelState.IsValid){
                
                entitiesGroup.Name = model.Name;
                entitiesGroup.Data = DateTime.Now;

                //UPDATE OR CREATE GROUP
                if(Id != 0){
                    //UPDATE GROUP
                    entitiesGroup.Id = Id;
                    _profilingGroupServices.UpdateProfilingGroups(entitiesGroup);

                    //DELETE RANGE OF SYSTEM MENU
                    _profilingGroupServices.DeleteProfilingGroupSystemMenu(Id);

                    //DELETE RANGE OF PROFILING OPERATOR IN THIS GROUP
                    _profilingGroupServices.DeleteProfilingOperatorGroup(Id);

                } else {
                    //CREATE GROUP
                    _profilingGroupServices.InsertProfilingGroups(entitiesGroup);
                }

                //INSERT PROFILING GROUP SYSTEM MENU
                foreach(var systemMenu in model.SystemMenuList){
                    if(systemMenu.Active){
                        var entitiesProfilingGroupSystemMenu = new ProfilingGroupSystemMenu();
                        entitiesProfilingGroupSystemMenu.Groups = entitiesGroup;
                        entitiesProfilingGroupSystemMenu.SystemMenus = _profilingGroupServices.GetSystemMenuById(systemMenu.Id);

                        _profilingGroupServices.InsertProfilingGroupsSystemMenu(entitiesProfilingGroupSystemMenu);
                    }
                }

                //INSERT PROFILING OPEATOR GROUP
                foreach(var Operator in model.OperatorsList){
                    if(Operator.Active){
                        var entitiesProfilingOperatorGroup = new ProfilingOperatorGroup();
                        entitiesProfilingOperatorGroup.Groups = entitiesGroup;
                        entitiesProfilingOperatorGroup.Operators = _operatorServices.GetOperator(Operator.Id);

                        _profilingGroupServices.InsertProfilingOperatorGroups(entitiesProfilingOperatorGroup);
                    }
                }

                //LOG
                var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
                var profilingOperator = _operatorServices.GetOperatorByUserId(UserId);
                if(Id != 0){
                    _logService.InsertSystemLog("Modifica", string.Concat("Modificato Gruppo Operatori: ", model.Name), profilingOperator);
                
                } else {
                    _logService.InsertSystemLog("Nuovo", string.Concat("Aggiunto Gruppo Operatori: ", model.Name), profilingOperator);
                }
            }
            
           return RedirectToAction("Groups");
        }

        public IActionResult GroupDelete(int id){
            
            //BEFORE DELETE RELATION
            _profilingGroupServices.DeleteProfilingGroupSystemMenu(id);

            //DELETE GROUP
            _profilingGroupServices.DeleteProfilingGroup(id);

            return RedirectToAction("Groups");
        }

        public IActionResult Emails(int pageSize = 10, int pageNumber = 1){
            ViewData["Title"] = "Account Email";
            var model = new EmailsTableViewModel();

            model.HowManyField = pageSize;
            model.HowManyFieldList = new List<EmailsTableViewModel.HowManyFields>{
                new EmailsTableViewModel.HowManyFields{ Value = 10, DisplayText="Visualizza 10 risultati per pagina" },
                new EmailsTableViewModel.HowManyFields{ Value = 100, DisplayText="Visualizza 100 risultati per pagina" },
                new EmailsTableViewModel.HowManyFields{ Value = 1000, DisplayText="Visualizza 1000 risultati per pagina" },
                new EmailsTableViewModel.HowManyFields{ Value = 100000, DisplayText="Visualizza tutti i risultati" }
            };

            //Pagination
            model.totalRecords = _emailServices.GetEmails().Count;
            model.pageNumber = pageNumber;
            model.pageSize = pageSize;
            model.pageTotal =  Math.Ceiling((double)model.totalRecords / pageSize);
            var excludeRecords = (pageSize * pageNumber) - pageSize;  
            model.displayRecord = model.pageSize * pageNumber;
            if(model.displayRecord > model.pageTotal){
                model.displayRecord = model.totalRecords;
            }

            if(model.totalRecords > pageSize){
                model.displayPagination = true;
            }

            var Emails = _emailServices.GetEmails(excludeRecords, pageSize);
            model.EmailsListViewModel = new List<EmailsViewModel>();
            
            foreach(var Email in Emails){
                model.EmailsListViewModel.Add(new EmailsViewModel{ 
                    Id = Email.Id, 
                    Name = Email.Name,
                    Address = Email.Email, 
                    Pop = Email.EmailPop,
                    Smtp = Email.EmailSmtp,
                    NUsing = Email.ProfilingOperatorEmails.Where(x => x.SystemEmails.Id == Email.Id).Count()
                });        
            }
            
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Emails(EmailsTableViewModel model, int pageNumber = 1){
            ViewData["Title"] = "Account Email";
            
            model.HowManyFieldList = new List<EmailsTableViewModel.HowManyFields>{
                new EmailsTableViewModel.HowManyFields{ Value = 10, DisplayText="Visualizza 10 risultati per pagina" },
                new EmailsTableViewModel.HowManyFields{ Value = 100, DisplayText="Visualizza 100 risultati per pagina" },
                new EmailsTableViewModel.HowManyFields{ Value = 1000, DisplayText="Visualizza 1000 risultati per pagina" },
                new EmailsTableViewModel.HowManyFields{ Value = 100000, DisplayText="Visualizza tutti i risultati" }
            };

            //Pagination
            model.totalRecords = _emailServices.GetEmailsByName(model.Find).Count;
            model.pageNumber = pageNumber;
            model.pageSize = model.HowManyField;
            model.pageTotal =  Math.Ceiling((double)model.totalRecords / model.HowManyField);
            var excludeRecords = (model.HowManyField * pageNumber) - model.HowManyField;  
            model.displayRecord = model.pageSize * pageNumber;
            if(model.displayRecord > model.pageTotal){
                model.displayRecord = model.totalRecords;
            }

            if(model.totalRecords > model.HowManyField){
                model.displayPagination = true;
            }

            var Emails = _emailServices.GetEmailsByName(model.Find, excludeRecords, model.pageSize);
            model.EmailsListViewModel = new List<EmailsViewModel>();
            
            foreach(var Email in Emails){
                model.EmailsListViewModel.Add(new EmailsViewModel{ 
                    Id = Email.Id, 
                    Name = Email.Name,
                    Address = Email.Email, 
                    Pop = Email.EmailPop,
                    Smtp = Email.EmailSmtp,
                    NUsing = Email.ProfilingOperatorEmails.Where(x => x.SystemEmails.Id == Email.Id).Count()
                });        
            }
            
            
            return View(model);
        }

        public IActionResult EmailsAddOrEdit(int Id){
             ViewData["Title"] = "Aggiungi Account Email";
            var model = new EmailAddViewModel();
            
            if(Id != 0){
                var entityEmail = _emailServices.GetSystemEmailById(Id);

                model.Id = entityEmail.Id;
                model.Name = entityEmail.Name;
                model.Email = entityEmail.Email;
                model.EmailPop = entityEmail.EmailPop;
                model.EmailPortPop = entityEmail.EmailPortPop;
                model.EmailSmtp = entityEmail.EmailSmtp;
                model.EmailPortSmtp = entityEmail.EmailPortSmtp;
                model.Username = _dataProtectionProvider.CreateProtector(Key).Unprotect(entityEmail.EmailUser);
                model.Password = _dataProtectionProvider.CreateProtector(Key).Unprotect(entityEmail.EmailPassword);
                model.Signature = entityEmail.EmailSignature;
                model.EmailSSLValue = entityEmail.EmailSSL;                

            } else {
                model.DefaultEmailConfig =  new[] { new SelectListItem { Value="", Text = "Seleziona un provider ..." } }.Concat(_emailServices.GetDefaultEmails().Select(x => new SelectListItem{
                    Value = x.Id.ToString(),
                    Text = x.EmailProvider
                }));
            }

            model.EmailSSL = new List<EmailSSL>{
                new EmailSSL{ Id = 1, CryptType = "Nessuna" },
                new EmailSSL{ Id = 2, CryptType = "SSL" },
                new EmailSSL{ Id = 3, CryptType = "TLS" }
            };
            

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EmailsAddOrEdit(EmailAddViewModel model, int Id){
            var entitesEmail = new SystemEmail();
            
            if(ModelState.IsValid){
                entitesEmail.Name = model.Name;
                entitesEmail.Email = model.Email;
                entitesEmail.EmailPop = model.EmailPop;
                entitesEmail.EmailPortPop = model.EmailPortPop;
                entitesEmail.EmailSmtp = model.EmailSmtp;
                entitesEmail.EmailPortSmtp = model.EmailPortSmtp;
                entitesEmail.EmailUser = _dataProtectionProvider.CreateProtector(Key).Protect(model.Username);
                entitesEmail.EmailPassword = _dataProtectionProvider.CreateProtector(Key).Protect(model.Password);
                entitesEmail.EmailSignature = model.Signature;
                entitesEmail.EmailSSL = model.EmailSSLValue;
            }

            if(Id != 0){
                entitesEmail.Id = Id;
                _emailServices.UpdateSystemEmail(entitesEmail);
            } else {
                _emailServices.InsertSystemEmail(entitesEmail);
            }
            
            //LOG
            var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
            var profilingOperator = _operatorServices.GetOperatorByUserId(UserId);
            if(Id != 0){
                _logService.InsertSystemLog("Modifica", string.Concat("Modificato Account Email: ", model.Email), profilingOperator);
            
            } else {
                _logService.InsertSystemLog("Nuovo", string.Concat("Aggiunto Account Email: ", model.Email), profilingOperator);
            }

            return RedirectToAction("Emails");
        }

        public SystemDefaultEmail GetDataEmailProvider(int Id){
            return _emailServices.GetDataEmailProvider(Id);
        }

        public IActionResult EmailDelete(int Id){
            _emailServices.DeleteSystemEmail(Id);
            
            return RedirectToAction("Emails");
        }
        
        public IActionResult Operators(int pageSize = 10, int pageNumber = 1){
            ViewData["Title"] = "Operatori";
            var model = new OperatorsTableViewModel();

            model.HowManyField = pageSize;
            model.HowManyFieldList = new List<EmailsTableViewModel.HowManyFields>{
                new EmailsTableViewModel.HowManyFields{ Value = 10, DisplayText="Visualizza 10 risultati per pagina" },
                new EmailsTableViewModel.HowManyFields{ Value = 100, DisplayText="Visualizza 100 risultati per pagina" },
                new EmailsTableViewModel.HowManyFields{ Value = 1000, DisplayText="Visualizza 1000 risultati per pagina" },
                new EmailsTableViewModel.HowManyFields{ Value = 100000, DisplayText="Visualizza tutti i risultati" }
            };

            //Pagination
            model.totalRecords = _operatorServices.GetOperators().Count;
            model.pageNumber = pageNumber;
            model.pageSize = pageSize;
            model.pageTotal =  Math.Ceiling((double)model.totalRecords / pageSize);
            var excludeRecords = (pageSize * pageNumber) - pageSize;  
            model.displayRecord = model.pageSize * pageNumber;
            if(model.displayRecord > model.pageTotal){
                model.displayRecord = model.totalRecords;
            }

            if(model.totalRecords > pageSize){
                model.displayPagination = true;
            }

            var Operators = _operatorServices.GetOperators(excludeRecords, pageSize);
            model.OperatatorListViewModel = new List<OperatorsViewModel>();
            
            foreach(var Operator in Operators){
                model.OperatatorListViewModel.Add(new OperatorsViewModel{ 
                    Id = Operator.Id,
                    UserId = Operator.UserId,
                    Name = Operator.Name,
                    LastName = Operator.Lastname,
                    EmailAddress = Operator.Email,
                    Group = Operator.ProfilingOperatorGroups.Any() ? _profilingGroupServices.GetProfilingGroupById(Operator.ProfilingOperatorGroups.FirstOrDefault().Groups.Id).Name : "",
                    PhoneNr = Operator.PhoneNr,
                    Enabled = Operator.Enabled == 1 ? "SI" : "NO"
                });        
            }
    
            return View(model);
        }

        [HttpPost]
        public IActionResult Operators(OperatorsTableViewModel model, int pageNumber = 1){
             ViewData["Title"] = "Operatori";

            model.HowManyFieldList = new List<EmailsTableViewModel.HowManyFields>{
                new EmailsTableViewModel.HowManyFields{ Value = 10, DisplayText="Visualizza 10 risultati per pagina" },
                new EmailsTableViewModel.HowManyFields{ Value = 100, DisplayText="Visualizza 100 risultati per pagina" },
                new EmailsTableViewModel.HowManyFields{ Value = 1000, DisplayText="Visualizza 1000 risultati per pagina" },
                new EmailsTableViewModel.HowManyFields{ Value = 100000, DisplayText="Visualizza tutti i risultati" }
            };

            //Pagination
            model.totalRecords = _operatorServices.GetOperatorsByName(model.Find).Count;
            model.pageNumber = pageNumber;
            model.pageSize = model.HowManyField;
            model.pageTotal =  Math.Ceiling((double)model.totalRecords / model.HowManyField);
            var excludeRecords = (model.HowManyField * pageNumber) - model.HowManyField;  
            model.displayRecord = model.pageSize * pageNumber;
            if(model.displayRecord > model.pageTotal){
                model.displayRecord = model.totalRecords;
            }

            if(model.totalRecords > model.HowManyField){
                model.displayPagination = true;
            }

            var Operators = _operatorServices.GetOperatorsByName(model.Find, excludeRecords, model.pageSize);
            model.OperatatorListViewModel = new List<OperatorsViewModel>();
            
            foreach(var Operator in Operators){
                model.OperatatorListViewModel.Add(new OperatorsViewModel{ 
                    Id = Operator.Id,
                    UserId = Operator.UserId,
                    Name = Operator.Name,
                    LastName = Operator.Lastname,
                    EmailAddress = Operator.Email,
                    Group = Operator.ProfilingOperatorGroups.Any() ? _profilingGroupServices.GetProfilingGroupById(Operator.ProfilingOperatorGroups.FirstOrDefault().Groups.Id).Name : "",
                    PhoneNr = Operator.PhoneNr,
                    Enabled = Operator.Enabled == 1 ? "SI" : "NO"
                });        
            }
    
            return View(model);
        }

        public IActionResult OperatorsAddOrEdit(int Id){
             ViewData["Title"] = "Aggiungi Operatori";
            var model = new OperatorAddViewModel();
            
            if(Id != 0){
                var entityOperator = _operatorServices.GetOperator(Id);
                
                model.Id = entityOperator.Id;
                model.UserId = entityOperator.UserId;
                model.Name = entityOperator.Name;
                model.LastName = entityOperator.Lastname;
                model.EmailAddress = entityOperator.Email;
                model.PhoneNr = entityOperator.PhoneNr;
                model.PhoneNr = entityOperator.PhoneNr;
                model.EnabledId = entityOperator.Enabled;
                model.GroupId = entityOperator.ProfilingOperatorGroups.Any() ? entityOperator.ProfilingOperatorGroups.FirstOrDefault().Groups.Id : 0;
                model.Password = entityOperator.Password;
                model.PasswordDeadLineValue = entityOperator.PasswordDeadline;
                
                foreach(var EmailMenage in entityOperator.ProfilingOperatorEmails){
                    model.EmailAccountId.Add(EmailMenage.Id);
                }

                var EmailsAccount = _operatorServices.GetListEmailAccount();
                foreach(var emailAccount in EmailsAccount){
                    var ActiveEmail = false;
                    if(entityOperator.ProfilingOperatorEmails.Where( x => x.SystemEmails.Id == emailAccount.Id).Any()){
                        ActiveEmail = true;
                    }

                    model.EmailsAccount.Add(new EmailAccount(){ Id = emailAccount.Id, Email = emailAccount.Email, Active = ActiveEmail });
                }

            } else {
                var EmailsAccount = _operatorServices.GetListEmailAccount();
                foreach(var emailAccount in EmailsAccount){
                     model.EmailsAccount.Add(new EmailAccount(){ Id = emailAccount.Id, Email = emailAccount.Email, Active = false });
                };
            }

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
        public IActionResult OperatorsAddOrEdit(OperatorAddViewModel model, int Id){
            var entitiesOperator = new ProfilingOperator();
            var operatorGuid = Guid.NewGuid();

            if(ModelState.IsValid){
                entitiesOperator.Guid = operatorGuid;
                entitiesOperator.Name = model.Name;
                entitiesOperator.Lastname = model.LastName;
                entitiesOperator.UserId = model.UserId;
                entitiesOperator.Email = model.EmailAddress;
                entitiesOperator.PhoneNr = model.PhoneNr;
                entitiesOperator.PasswordDeadline = model.PasswordDeadLineValue;
                entitiesOperator.OperatorData = DateTime.Now;
                entitiesOperator.PasswordLastEdit = DateTime.Now;
            }

            if(Id != 0){
                //CHECK EXIST OPERATOR
                if(_operatorServices.ExistOperator(model.UserId, model.EmailAddress)){
                    //UPDATE OPERATOR
                    entitiesOperator.Id = Id;
                    _operatorServices.UpdateOperator(entitiesOperator);

                    //DELETE EMAIL ACCOUNT MANAGEMENT
                    _operatorServices.DeleteProfilingOperatorEmail(Id);
                }
            } else {
                
                //CHECK EMAIL AND USERID IS NOT USED 
                if(_operatorServices.ExistOperator(model.UserId, model.EmailAddress)){
                    var errorViewModel = new ErrorViewModel();
                    errorViewModel.Error = "Email o UserID Presenti";
                    return View("500", errorViewModel);
                }

                // INSERT OPERATOR
                _operatorServices.InsertOperator(entitiesOperator);

                //INSERT OPERATOR GROUP
                var profilingOperatorGroup = new ProfilingOperatorGroup();
                profilingOperatorGroup.Operators = entitiesOperator;
                profilingOperatorGroup.Groups = _profilingGroupServices.GetProfilingGroupById(model.GroupId);
                _operatorServices.InsertProfilingOperatorGroup(profilingOperatorGroup);

                //SEND EMAIL ACTIVE ACCOUNT
                var EmailSender = new EmailSenderViewModel();
                EmailSender.EmailTemplate = Path.GetFullPath("EmailTemplate/EmailTemplate.html");
                EmailSender.EmailObject = "FLM Auto & Rent | Completa Il Tuo Account";
                EmailSender.EmailTo = model.EmailAddress;
                EmailSender.EmailBody = String.Concat("Ciao ", model.Name, " ", model.LastName, " (UserID: ", model.UserId, ")", "<br /> Ti invitiamo a collegarti al seguente link:<br /><a target='_blank' style='padding: 0; margin: 0; width: auto; height: auto; background: none trasparent; border: 0px none trasparent; outline: 0px none; font-size: 18px; line-height: 36px; text-align: left; font-weight: 400; text-decoration: none; color: #1E88E5; vertical-align: top;' href='", this.Request.Scheme, "://", this.Request.Host, this.Request.PathBase,  "/Admin/Account/Password/",operatorGuid, "'>",   this.Request.Scheme, "://", this.Request.Host, this.Request.PathBase,  "/Admin/Account/Password/",operatorGuid, "</a><br /> Per completare la registrazione al Gestionale FLM Auto & Rent<br /><br /> Lo Staff");

                _emailServices.EmailSender(EmailSender.EmailTemplate, EmailSender.EmailObject, EmailSender.EmailTo, EmailSender.EmailBody);
            }

            //INSERT EMAIL ACCOUNT MANAGEMENT
            foreach(var emailAccount in model.EmailsAccount){
                if(emailAccount.Active){
                    var entitiesProfilingOperatorEmail = new ProfilingOperatorEmail();
                    entitiesProfilingOperatorEmail.Operators = entitiesOperator;
                    entitiesProfilingOperatorEmail.SystemEmails = _operatorServices.GetListEmailAccountById(emailAccount.Id);

                    _operatorServices.InsertProfilingOperatorEmail(entitiesProfilingOperatorEmail);
                }
            }

            //LOG
            var UserId = _dataProtectionProvider.CreateProtector(Key).Unprotect(User.Identity.Name);
            var profilingOperator = _operatorServices.GetOperatorByUserId(UserId);
            if(Id != 0){
                _logService.InsertSystemLog("Modifica", string.Concat("Modificato Account Email: ", model.UserId), profilingOperator);
            
            } else {
                _logService.InsertSystemLog("Nuovo", string.Concat("Aggiunto Account Email: ", model.UserId), profilingOperator);
            }
            
            return RedirectToAction("Operators");
        }

        public IActionResult OperatorResetPassword(int Id){
            var operatorGuid = Guid.NewGuid();
            var ProfilingOperator = _operatorServices.GetOperator(Id);
            
            //CHECK EXIST OPERATOR
            if(_operatorServices.ExistOperator(ProfilingOperator.UserId, ProfilingOperator.Email)){
                _operatorServices.ResetProfilingOperatorPassword(ProfilingOperator, operatorGuid);
                
                //SEND EMAIL ACTIVE ACCOUNT
                var EmailSender = new EmailSenderViewModel();
                EmailSender.EmailTemplate = Path.GetFullPath("EmailTemplate/EmailTemplate.html");
                EmailSender.EmailObject = "FLM Auto & Rent | Reset Password";
                EmailSender.EmailTo = ProfilingOperator.Email;
                EmailSender.EmailBody = String.Concat("Ciao ", ProfilingOperator.Name, " ", ProfilingOperator.Lastname, " (UserID: ", ProfilingOperator.UserId, ")", "<br /> Ti invitiamo a collegarti al seguente link:<br /><a target='_blank' style='padding: 0; margin: 0; width: auto; height: auto; background: none trasparent; border: 0px none trasparent; outline: 0px none; font-size: 18px; line-height: 36px; text-align: left; font-weight: 400; text-decoration: none; color: #1E88E5; vertical-align: top;' href='", this.Request.Scheme, "://", this.Request.Host, this.Request.PathBase,  "/Admin/Account/Password/",operatorGuid, "'>",   this.Request.Scheme, "://", this.Request.Host, this.Request.PathBase,  "/Admin/Account/Password/",operatorGuid, "</a><br /> Per resettare la password di accesso al Gestionale FLM Auto & Rent<br /><br /> Lo Staff");

                _emailServices.EmailSender(EmailSender.EmailTemplate, EmailSender.EmailObject, EmailSender.EmailTo, EmailSender.EmailBody);    
            }
            
            return RedirectToAction("Operators");
        }
    
        public IActionResult OperatorDelete(int Id){
            //DELETE OPERATOR - EMAIL 
            _operatorServices.DeleteProfilingOperatorEmail(Id);

            //DELETE OPERATOR - GROUPS 
            _operatorServices.DeleteProfilingOperatorGroup(Id);

            //DELETE OPERATOR
            _operatorServices.DeleteSystemOperator(Id);
            
            return RedirectToAction("Operators");
        }
        
    }
}