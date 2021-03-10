﻿using System;
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

    public interface IProfilingGroupServices
    {
        //GET
        ProfilingSystemMenu GetSystemMenuById(int id);
        IList<ProfilingSystemMenu> GetSystemMenus(string userId = null);
        IList<ProfilingSystemMenu> GetSystemMenusHeader(string userId = null);
        IList<ProfilingSystemMenu> GetSystemMenusAside(string path, string userId = null);
        IList<ProfilingGroup> GetProfilingGroups();
        ProfilingGroup GetProfilingGroupById(int id);

        //CRUD
        void InsertProfilingGroups(ProfilingGroup model);
        void InsertProfilingGroupsSystemMenu(ProfilingGroupSystemMenu model);
        void UpdateProfilingGroups(ProfilingGroup model);
        void DeleteProfilingGroup(int id);
        void DeleteProfilingGroupSystemMenu(int ProfilingGroupId);
        
        void InsertProfilingOperatorGroups(ProfilingOperatorGroup model);
        void DeleteProfilingOperatorGroup(int ProfilingGroupId);

    }

    public class ProfilingGroupServices : IProfilingGroupServices
    {
        private readonly FlmAutoRentContext _ctx;
        
        public ProfilingGroupServices(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }

        public virtual ProfilingSystemMenu GetSystemMenuById(int id){
            return _ctx.ProfilingSystemMenus.Where(sm => sm.Id == id).FirstOrDefault();
        }

        public virtual IList<ProfilingSystemMenu> GetSystemMenus(string userId = null){
            if(userId != null){
                //GET profilingOperatorGroups BY UserID Logged
                var profilingOperatorGroups = _ctx.ProfilingOperatorGroups.Include(x => x.Groups).FirstOrDefault(x => x.Operators.UserId == userId);

                //GET ProfilingGroupSystemMenus
                var ProfilingGroupSystemMenus = _ctx.ProfilingGroupSystemMenus.Include(x => x.SystemMenus).Where(x => x.Groups.Id == profilingOperatorGroups.Groups.Id);

                //RETURN ProfilingSystemMenus LIST
                return _ctx.ProfilingSystemMenus.Where(hm => ProfilingGroupSystemMenus.Any(x => x.SystemMenus.Id == hm.Id)).OrderBy(sm => sm.Id).ThenBy(sm => sm.MenuFatherId).ThenBy(sm => sm.Priority).ToList();
            }

            return _ctx.ProfilingSystemMenus.OrderBy(sm => sm.Id).ThenBy(sm => sm.MenuFatherId).ThenBy(sm => sm.Priority).ToList();
        }
        
        public virtual IList<ProfilingSystemMenu> GetSystemMenusHeader(string userId = null){
            if(userId != null){
                //GET profilingOperatorGroups BY UserID Logged
                var profilingOperatorGroups = _ctx.ProfilingOperatorGroups.Include(x => x.Groups).FirstOrDefault(x => x.Operators.UserId == userId);
                
                //GET ProfilingGroupSystemMenus
                var ProfilingGroupSystemMenus = _ctx.ProfilingGroupSystemMenus.Include(x => x.SystemMenus).Where(x => x.Groups.Id == profilingOperatorGroups.Groups.Id);

                //RETURN ProfilingSystemMenus LIST
                return _ctx.ProfilingSystemMenus.Where(hm => hm.MenuFatherId == null && ProfilingGroupSystemMenus.Any(x => x.SystemMenus.Id == hm.Id)).OrderBy(x => x.Priority).ToList();
            }
            
            return _ctx.ProfilingSystemMenus.Where(hm => hm.MenuFatherId == null).OrderBy(x => x.Priority).ToList();
        }

        public virtual IList<ProfilingSystemMenu> GetSystemMenusAside(string path, string userId = null){
            var fatherId = _ctx.ProfilingSystemMenus.FirstOrDefault(x => x.MenuFatherId == null && path.Contains(x.CodMenu)).Id;
            if(userId != null){
                //GET profilingOperatorGroups BY UserID Logged
                var profilingOperatorGroups = _ctx.ProfilingOperatorGroups.Include(x => x.Groups).FirstOrDefault(x => x.Operators.UserId == userId);
                
                //GET ProfilingGroupSystemMenus
                var ProfilingGroupSystemMenus = _ctx.ProfilingGroupSystemMenus.Include(x => x.SystemMenus).Where(x => x.Groups.Id == profilingOperatorGroups.Groups.Id);

                //RETURN ProfilingSystemMenus LIST
                return _ctx.ProfilingSystemMenus.Where(am => am.MenuFatherId == fatherId && ProfilingGroupSystemMenus.Any(x => x.SystemMenus.Id == am.Id)).OrderBy(x => x.Priority).ToList();
            }
            
            return _ctx.ProfilingSystemMenus.Where(am => am.MenuFatherId == fatherId).OrderBy(x => x.Priority).ToList();
        }        

        public virtual IList<ProfilingGroup> GetProfilingGroups(){
            return _ctx.ProfilingGroups.Include(x => x.ProfilingOperatorGroups).ToList();
        }    

        public virtual ProfilingGroup GetProfilingGroupById(int Id){
            return _ctx.ProfilingGroups.Include(x => x.ProfilingGroupSystemMenus).FirstOrDefault(x=> x.Id == Id);
        }    

        public void InsertProfilingGroups(ProfilingGroup model){
            _ctx.ProfilingGroups.Add(model);
            _ctx.SaveChanges();
        }

        public void InsertProfilingGroupsSystemMenu(ProfilingGroupSystemMenu model){
            _ctx.ProfilingGroupSystemMenus.Add(model);
            _ctx.SaveChanges();
        }

        public void UpdateProfilingGroups(ProfilingGroup model){
            var ProfilingGroup = _ctx.ProfilingGroups.Find(model.Id);

            ProfilingGroup.Name = model.Name;
            _ctx.SaveChanges();
        }

        public void DeleteProfilingGroup(int id){
            var ProfilingGroup = _ctx.ProfilingGroups.Where(x => x.Id == id);
            _ctx.ProfilingGroups.RemoveRange(ProfilingGroup);

            _ctx.SaveChanges();
        }

        public void DeleteProfilingGroupSystemMenu(int ProfilingGroupId){
            var SystemsMenusToBeDelete = _ctx.ProfilingGroupSystemMenus.Where(x => x.Groups.Id == ProfilingGroupId);
            
            _ctx.ProfilingGroupSystemMenus.RemoveRange(SystemsMenusToBeDelete);
            _ctx.SaveChanges();
        }

        public void InsertProfilingOperatorGroups(ProfilingOperatorGroup model){
            _ctx.ProfilingOperatorGroups.Add(model);
            _ctx.SaveChanges();
        }
        public void DeleteProfilingOperatorGroup(int ProfilingGroupId){
            var ProfilingOperatorGroupToBeDelete = _ctx.ProfilingOperatorGroups.Where(x => x.Groups.Id == ProfilingGroupId);
            
            _ctx.ProfilingOperatorGroups.RemoveRange(ProfilingOperatorGroupToBeDelete);
            _ctx.SaveChanges();
        }

    }
}
