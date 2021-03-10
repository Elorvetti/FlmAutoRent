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
    public interface IPeopleService 
    {
        People GetPeople(string email);
        void InsertPeople(People model);
        void InsertPeopleMessage(PeopleMessage model);
    }
    public class PeopleService : IPeopleService
    {
        private readonly FlmAutoRentContext _ctx;
        public PeopleService(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }
        public People GetPeople(string email){
            return _ctx.Peoples.FirstOrDefault(x => x.Email == email);
        }

        public void InsertPeople(People model){
            _ctx.Peoples.Add(model);
            _ctx.SaveChanges();
        }

        public void InsertPeopleMessage(PeopleMessage model){
            _ctx.PeopleMessages.Add(model);
            _ctx.SaveChanges();
        }
    }
}