using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using FlmAutoRent.Data;
using FlmAutoRent.Data.Entities;


namespace FlmAutoRent.Services
{
    public interface IEmailServices
    {
        IList<SystemEmail> GetEmails(int excludeRecord = 0, int pageSize = int.MaxValue);
        IList<SystemEmail> GetEmailsByName(string find, int excludeRecord = 0, int pageSize = int.MaxValue);
        IList<SystemDefaultEmail> GetDefaultEmails();
        SystemDefaultEmail GetDataEmailProvider(int Id);
        void InsertSystemEmail(SystemEmail model);
        SystemEmail GetSystemEmailById(int Id);
        IList<SystemEmail> GetSystemEmailsByListId(IQueryable<int> Id);
        void UpdateSystemEmail(SystemEmail model);
        void DeleteSystemEmail(int Id);
        void EmailSender(string EmailTemplate, string EmailObject, string EmailTo, string EmailBody);
    }

    public class EmailServices : IEmailServices 
    {
        private readonly FlmAutoRentContext _ctx;
        
        public EmailServices(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }

        public virtual IList<SystemEmail> GetEmails(int excludeRecord = 0, int pageSize = int.MaxValue){
            return _ctx.SystemEmails.Include(x => x.ProfilingOperatorEmails).Skip(excludeRecord).Take(pageSize).ToList();
        }   

        public virtual IList<SystemEmail> GetEmailsByName(string find, int excludeRecord = 0, int pageSize = int.MaxValue){
            return _ctx.SystemEmails.Include(x => x.ProfilingOperatorEmails).Where(x => EF.Functions.Like(x.Email, string.Concat("%", find, "%")) || EF.Functions.Like(x.Name, string.Concat("%", find, "%")) ).Skip(excludeRecord).Take(pageSize).ToList();
        }

        public virtual IList<SystemDefaultEmail> GetDefaultEmails(){
            return _ctx.SystemDefaultEmails.ToList();
        }

        public virtual SystemDefaultEmail GetDataEmailProvider(int Id){
            return _ctx.SystemDefaultEmails.FirstOrDefault(x => x.Id == Id);
        }

        public void InsertSystemEmail(SystemEmail model){
            _ctx.SystemEmails.Add(model);
            _ctx.SaveChanges();
        }
        
        public SystemEmail GetSystemEmailById(int Id){
            return _ctx.SystemEmails.FirstOrDefault(x => x.Id == Id);
        }

        public IList<SystemEmail> GetSystemEmailsByListId(IQueryable<int> Id){
            return _ctx.SystemEmails.Where(x => Id.Contains(x.Id)).ToList();
        }

        public void UpdateSystemEmail(SystemEmail model){
            var systemEmail = _ctx.SystemEmails.Find(model.Id);

            systemEmail.Name = model.Name;
            systemEmail.Email = model.Email;
            systemEmail.EmailPop = model.EmailPop;
            systemEmail.EmailPortPop = model.EmailPortPop;
            systemEmail.EmailSmtp = model.EmailSmtp;
            systemEmail.EmailPortSmtp = model.EmailPortSmtp;
            systemEmail.EmailUser = model.EmailUser;
            systemEmail.EmailPassword = model.EmailPassword;
            systemEmail.EmailSignature = model.EmailSignature;
            systemEmail.EmailSSL = model.EmailSSL;

            _ctx.SaveChanges();
        }

        public void DeleteSystemEmail(int Id){
            var systemEmail = _ctx.SystemEmails.Where(x => x.Id == Id);
            _ctx.SystemEmails.RemoveRange(systemEmail);

            _ctx.SaveChanges();
        }

        public void EmailSender(string EmailTemplate, string EmailObject, string EmailTo, string EmailBody){
            string body = string.Empty;

            var path = EmailTemplate;
           
            //Read email template and replace value with model value
            StreamReader reader = new StreamReader(path);
            body = reader.ReadToEnd();
            body = body.Replace("[OGGETTO]", EmailObject);
            body = body.Replace("[TESTO]", EmailBody);
            body = body.Replace("[FIRMA]", "Lo staff");
            
            //configure email data
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("info@flmautorent.com");
            mailMessage.To.Add(new MailAddress(EmailTo));
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = EmailObject;

            //Configure Client
            SmtpClient client = new SmtpClient("ssl0.ovh.net");
            client.Port = 587;
            client.Credentials = new NetworkCredential("info@flmautorent.com", "OhyR3e9rPuMavXAyd3yX");
            client.EnableSsl = false;
            client.Send(mailMessage);

        }

    }
}