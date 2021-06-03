using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using FlmAutoRent.Data;
using FlmAutoRent.Data.Entities;


namespace FlmAutoRent.Services
{
    public interface IOperatorServices
    {
        bool ExistOperatorGuid(Guid guid);
        bool ExistOperator(string userdId, string email);
        bool LoginOperatorSuccess(string userId, string password);
        ProfilingOperator LoginOperator(string userId, string password);
        IList<ProfilingOperator> GetOperators(int excludeRecord = 0, int pageSize = int.MaxValue);
        IList<ProfilingOperator> GetOperatorsByName(string find, int excludeRecord = 0, int pageSize = int.MaxValue);
        IList<ProfilingGroup> GetListGroup();
        IList<SystemEmail> GetListEmailAccount();
        SystemEmail GetListEmailAccountById(int id);
        void InsertOperator(ProfilingOperator model);
        ProfilingOperator GetOperator(int Id);
        ProfilingOperator GetOperatorByUserId(string userID);
        void UpdateOperator(ProfilingOperator model);
        void DeleteOperator(int Id);
        void InsertProfilingOperatorGroup(ProfilingOperatorGroup model);
        void UpdateProfilingOperatorGroup(ProfilingOperatorGroup model);
        void DeleteProfilingOperatorGroup(int ProfilingOperatorId);        
        void InsertProfilingOperatorEmail(ProfilingOperatorEmail model);
        void DeleteProfilingOperatorEmail(int ProfilingOperatorId);
        void UpdateProfilingOperatorPassword(Guid AccountGuid, string Password);
        void ResetProfilingOperatorPassword(ProfilingOperator model, Guid AccountGuid);
        void DeleteSystemOperator(int id);
    }

    public class OperatorServices : IOperatorServices 
    {
        private readonly FlmAutoRentContext _ctx;
        
        public OperatorServices(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }

        public bool ExistOperatorGuid(Guid guid){
            if(_ctx.ProfilingOperators.Where(x => x.Guid == guid).Any()){
                return true;
            }    

            return false;
        }

        public bool ExistOperator(string userdId, string email){
            if(_ctx.ProfilingOperators.Where(x => x.UserId == userdId || x.Email == email).Any()){
                return true;
            }

            return false;
        }

        public bool LoginOperatorSuccess(string userId, string password){
            if(_ctx.ProfilingOperators.Where(x => x.Enabled == 1 && (x.UserId == userId && x.Password == password)).Any()){
                return true;
            }

            return false;
        }

        public ProfilingOperator LoginOperator(string userId, string password){
            return _ctx.ProfilingOperators.Include(x => x.ProfilingOperatorGroups).ThenInclude(x => x.Groups).FirstOrDefault(x => x.UserId == userId || x.Password == password);
        }

        public virtual IList<ProfilingOperator> GetOperators(int excludeRecord = 0, int pageSize = int.MaxValue){
            return _ctx.ProfilingOperators.Include(x => x.ProfilingOperatorGroups).ThenInclude(x => x.Groups).Skip(excludeRecord).Take(pageSize).ToList();
        }   

        public virtual IList<ProfilingOperator> GetOperatorsByName(string find, int excludeRecord = 0, int pageSize = int.MaxValue){
            return _ctx.ProfilingOperators.Include(x => x.ProfilingOperatorGroups).ThenInclude(x => x.Groups).Where( x => EF.Functions.Like(x.Lastname, string.Concat("%", find, "%")) || EF.Functions.Like(x.Name, string.Concat("%", find, "%")) || EF.Functions.Like(x.Email, string.Concat("%", find, "%")) ).Skip(excludeRecord).Take(pageSize).ToList();
        }

        public virtual IList<ProfilingGroup> GetListGroup(){
            return _ctx.ProfilingGroups.ToList();
        }

        public virtual IList<SystemEmail> GetListEmailAccount(){
            return _ctx.SystemEmails.ToList();
        }

        public virtual SystemEmail GetListEmailAccountById(int id){
            return _ctx.SystemEmails.Where(x => x.Id == id).FirstOrDefault();
        }

        public void InsertOperator(ProfilingOperator model){
            _ctx.ProfilingOperators.Add(model);
            _ctx.SaveChanges();
        }

        public ProfilingOperator GetOperator(int Id){
            return _ctx.ProfilingOperators.Include(x => x.ProfilingOperatorGroups).ThenInclude(x => x.Groups).Include(x => x.ProfilingOperatorEmails).ThenInclude( x => x.SystemEmails).FirstOrDefault(x => x.Id == Id);
        }

        public ProfilingOperator GetOperatorByUserId(string userID){
            return _ctx.ProfilingOperators.Include(x => x.ProfilingOperatorGroups).ThenInclude(x => x.Groups).FirstOrDefault(x => x.UserId == userID );
        }
        public void UpdateOperator(ProfilingOperator model){
            var Operator = GetOperator(model.Id);
            
            Operator.Name = model.Name;
            Operator.Lastname = model.Lastname;
            Operator.Email = model.Email;
            Operator.PhoneNr = model.PhoneNr;
            Operator.Enabled = model.Enabled;
            
            _ctx.SaveChanges();
        }

        public void DeleteOperator(int Id){
            var Operator = _ctx.ProfilingOperators.Where(x => x.Id == Id);
            _ctx.ProfilingOperators.RemoveRange(Operator);

            _ctx.SaveChanges();
        }

        public void InsertProfilingOperatorGroup(ProfilingOperatorGroup model){
            _ctx.ProfilingOperatorGroups.Add(model);
            _ctx.SaveChanges();
        }

        public void UpdateProfilingOperatorGroup(ProfilingOperatorGroup model){
            var profilingOperatorGroup = _ctx.ProfilingOperatorGroups.FirstOrDefault(x => x.Id == model.Id);
            
            profilingOperatorGroup.Groups = model.Groups;
            profilingOperatorGroup.Operators = model.Operators;

            _ctx.SaveChanges();
        }

        public void DeleteProfilingOperatorGroup(int ProfilingOperatorId){
            var profilingOperatorGroup = _ctx.ProfilingOperatorGroups.Where(x => x.Operators.Id == ProfilingOperatorId);
            _ctx.ProfilingOperatorGroups.RemoveRange(profilingOperatorGroup);

            _ctx.SaveChanges();
;        }
 
        public void InsertProfilingOperatorEmail(ProfilingOperatorEmail model){
            _ctx.ProfilingOperatorEmails.Add(model);
            _ctx.SaveChanges();
        }

        public void DeleteProfilingOperatorEmail(int ProfilingOperatorId){
            var profilingOperatorEmail = _ctx.ProfilingOperatorEmails.Where(x => x.Operators.Id == ProfilingOperatorId);
            _ctx.ProfilingOperatorEmails.RemoveRange(profilingOperatorEmail);

            _ctx.SaveChanges();
        }

        public void UpdateProfilingOperatorPassword(Guid AccountGuid, string Password){
            var ProfilingOperator = _ctx.ProfilingOperators.FirstOrDefault(x => x.Guid == AccountGuid);
            ProfilingOperator.Password = Password;
            ProfilingOperator.PasswordLastEdit = DateTime.Now;
            ProfilingOperator.Enabled = 1;
            ProfilingOperator.Guid = Guid.Empty;

            _ctx.SaveChanges();
        }

        public void ResetProfilingOperatorPassword(ProfilingOperator model, Guid AccountGuid){
            var ProfilingOperator = _ctx.ProfilingOperators.FirstOrDefault(x => x.Id == model.Id);
            ProfilingOperator.Password = string.Empty;
            ProfilingOperator.Enabled = 0;
            ProfilingOperator.Guid = AccountGuid;

            _ctx.SaveChanges();
        }

        public void DeleteSystemOperator(int id){
            var profilingOperator = _ctx.ProfilingOperators.Where(x => x.Id == id);
            _ctx.ProfilingOperators.RemoveRange(profilingOperator);

            _ctx.SaveChanges();
        }
    }
}