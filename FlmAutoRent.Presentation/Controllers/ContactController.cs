using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FlmAutoRent.Presentation.Models;
using FlmAutoRent.Services;
using FlmAutoRent.Data.Entities;

namespace FlmAutoRent.Presentation.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPeopleService _peopleService;
        private readonly ICarServices _carService;
        private readonly IEmailServices _emailService;

        public ContactController(ILogger<HomeController> logger, IPeopleService peopleService, ICarServices carService, IEmailServices emailService)
        {
            this._logger = logger;
            this._peopleService = peopleService;
            this._carService = carService;
            this._emailService = emailService;

        }

        public IActionResult Index(){
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(VehicleViewModel model)
        {            
            ViewData["Title"] = "Thank You Page";
            ViewData["MetaTitle"] = "Thank You Page";
            ViewData["MetaDescription"] = "Thank You Page";

            if(ModelState.IsValid){
                //1. INSERIRE NUOVA ANAGRAFICA
                var entityPeople = new People();
                entityPeople.Name = model.ContactModal.Name;
                entityPeople.Lastname = model.ContactModal.Lastname;
                entityPeople.Email = model.ContactModal.Email;


                //CONTROLLO PRESENZA ANAGRAFICA - SE PRESENTE PREDO DATI ANAGRAFICA
                if(_peopleService.GetPeople(entityPeople.Email) == null){
                    _peopleService.InsertPeople(entityPeople);
                } else {
                    entityPeople = _peopleService.GetPeople(entityPeople.Email) ;
                }

                //GET DAI VEICOLO
                var vehicle = _carService.GetVehicleById(model.Id);
                var vehicleBrand = vehicle.VehiclesMappings.FirstOrDefault(x => x.Vehicles.Id == vehicle.Id).Brands.BrandName;
                

                //2. INSERIRE MESSAGGIO CON RIFERIMENTO AD ANAGRAFICA
                var entityPeopleMessage = new PeopleMessage();
                entityPeopleMessage.People = entityPeople;
                entityPeopleMessage.Vehicle = vehicle;
                entityPeopleMessage.Message = model.ContactModal.Message;
                entityPeopleMessage.DateSend = DateTime.Now;
                _peopleService.InsertPeopleMessage(entityPeopleMessage);

                //3. INVIO EMAIL DI RINGRAZIAMENTO 
                var EmailSender = new EmailSenderViewModel();
                EmailSender.EmailTemplate = Path.GetFullPath("EmailTemplate/EmailTemplate.html");
                EmailSender.EmailObject = "RICHIESTA CONTATTO | FLM Auto & Rent";
                EmailSender.EmailTo = model.ContactModal.Email;
                EmailSender.EmailBody = String.Concat("Gentile ", model.ContactModal.Name, " ",model.ContactModal.Lastname, "<br />La ringraziamo per aver scelto FLM Auto & Rent, abbiamo preso in carico la sua richiesta per il veicolo: <strong>",vehicleBrand, " ", vehicle.Model,"</strong><br />La contatteremo il prima possibile.<br /><br />");

                _emailService.EmailSender(EmailSender.EmailTemplate, EmailSender.EmailObject, EmailSender.EmailTo, EmailSender.EmailBody);

                //4. INVIO EMAIL DI RICHIESTA CONTATTO
                var EmailSenderRequest = new EmailSenderViewModel();
                EmailSenderRequest.EmailTemplate = Path.GetFullPath("EmailTemplate/EmailTemplate.html");
                EmailSenderRequest.EmailObject = "RICHIESTA CONTATTO | FLM Auto & Rent";
                EmailSenderRequest.EmailTo = "manu.lorv@gmail.com";
                EmailSenderRequest.EmailBody = String.Concat("Richiesta Contatto da: ",model.ContactModal.Name, " ", model.ContactModal.Lastname, "<br /> Indirizzo Email: ", model.ContactModal.Email, "<br />", vehicleBrand, " - ", vehicle.Model, "<br />", "Data richiesta: ", model.ContactModal.StartRent, " - ", model.ContactModal.EndRent, "<br />Messaggio: ", model.ContactModal.Message );

                _emailService.EmailSender(EmailSenderRequest.EmailTemplate, EmailSenderRequest.EmailObject, EmailSenderRequest.EmailTo, EmailSenderRequest.EmailBody);

                //5. REDIRECT THANK YOU PAGE
                entityPeople = new People();
                return View("~/Views/Vehicle/ThankYouPage.cshtml");
            }


            return View("500");
        }

    }
}
