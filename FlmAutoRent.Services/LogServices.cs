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
    public interface ILogServices 
    {
        IList<SystemLog> GetSystemLogsByOperatorId(ProfilingOperator model);
        void InsertSystemLog(string EventType, string EventValue, ProfilingOperator profilingOperator);
    }
    public class LogServices : ILogServices
    {
        private readonly FlmAutoRentContext _ctx;
        
        public LogServices(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }

        public IList<SystemLog> GetSystemLogsByOperatorId(ProfilingOperator model){
            return _ctx.SystemLogs.Include(x => x.Operators).Where(x => x.Operators.Id == model.Id).ToList();
        }

        public void InsertSystemLog(string EventType, string EventValue, ProfilingOperator profilingOperator){
            var model = new SystemLog();
            model.Data = DateTime.Now;
            model.EventType = EventType;
            model.Value = EventValue;
            model.Operators= profilingOperator;
            
            _ctx.SystemLogs.Add(model);
            _ctx.SaveChanges();
        }

    }
}