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
    public interface IVehiclesBrandsServices {
        IList<SeoIndex> GetListSeoIndex();
        SeoIndex GetContentNewsSeoIndexById(int Id);
        IList<VehiclesBrand> GetVehiclesBrands();
        VehiclesBrand GetVehiclesBrandById(int id);
        void InsertBrand(VehiclesBrand model);
        void UpdateBrand(VehiclesBrand model);
        void DeleteBrand(int id);
    }

    public class VehiclesBrandsServices : IVehiclesBrandsServices
    {
        private readonly FlmAutoRentContext _ctx;

        public VehiclesBrandsServices(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }
        
        public IList<VehiclesBrand> GetVehiclesBrands(){
            return _ctx.VehiclesBrands.Include(x => x.VehiclesMappings).ToList();
        }

        public VehiclesBrand GetVehiclesBrandById(int id){
            return _ctx.VehiclesBrands.Include(x => x.SeoIndex).FirstOrDefault(x => x.Id == id);
        }

        public IList<SeoIndex> GetListSeoIndex(){
            return _ctx.SeoIndex.ToList();
        }        

        public SeoIndex GetContentNewsSeoIndexById(int Id){
            return _ctx.SeoIndex.FirstOrDefault(x => x.Id == Id);
        }
        
        public void InsertBrand(VehiclesBrand model){
            _ctx.VehiclesBrands.Add(model);
            _ctx.SaveChanges();
        }

        public void UpdateBrand(VehiclesBrand model){
            var entitiesBrand = _ctx.VehiclesBrands.Find(model.Id);

            entitiesBrand.BrandName = model.BrandName;
            entitiesBrand.Description = model.Description;   
            entitiesBrand.OperatorData = model.OperatorData;
            entitiesBrand.IDOperator = model.IDOperator;
            entitiesBrand.PermaLink = model.PermaLink;
            entitiesBrand.MetaDescription = model.MetaDescription;
            entitiesBrand.MetaTitle = model.MetaTitle;
            entitiesBrand.SeoIndexRef = model.SeoIndexRef;

            _ctx.SaveChanges();
        }

        public void DeleteBrand(int id){
            var brand = _ctx.VehiclesBrands.Where( x => x.Id == id);
            _ctx.VehiclesBrands.RemoveRange(brand);

            _ctx.SaveChanges();
        }
    }
}