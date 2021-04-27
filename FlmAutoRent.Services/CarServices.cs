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
    public interface ICarServices {
        IList<Vehicle> GetVehicles(int excludeRecord = 0, int pageSize = int.MaxValue);
        IList<Vehicle> GetVehiclesByName(string find, int excludeRecord = 0, int pageSize = int.MaxValue);
        Vehicle GetVehicleById(int Id);
        IList<VehiclePowerSupply> GetVehiclePowerSupply();
        VehiclePowerSupply GetVehiclePowerSupplyById(int Id);
        IList<ContentCategory> GetVehicleCategory();
        IList<VehiclesImagesMapping> GetVehiclesImages(int VehicleId);
        VehiclesImage GetVehiclesImagesById(int Id);
        SeoIndex GetContentNewsSeoIndexById(int Id);
        IList<PeopleMessage> GetVehicleContact(int vehicleId);
        void InsertFirstStepVehicle(Vehicle model);
        void UpdateFirstStepVehicle(Vehicle model);
        void UpdateVehicleStep(Vehicle model);
        void UpdateVehicleSEO(Vehicle model);
        void InsertVehicleCategory(ContentCategoryNews model);
        void DeleteVehicleCategory(int VehicleId);
        void InsertVehicleMapping(VehiclesMapping model);
        void DeleteVehicleMapping(int VehicleId);
        void InsertVehicleImage(VehiclesImage model);
        void UpdateVehicleImage(VehiclesImage model);
        void DeleteVehicleImage(int ImageId);
        void InsertVehicleMapping(VehiclesImagesMapping model);
    }

    public class CarServices : ICarServices
    {
        private readonly FlmAutoRentContext _ctx;

        public CarServices(FlmAutoRentContext ctx){
            this._ctx = ctx;
        }

        public IList<Vehicle> GetVehicles(int excludeRecord = 0, int pageSize = int.MaxValue){
            return _ctx.Vehicles.Include(x => x.VehiclesMappings).ThenInclude(x => x.Brands).Include(x => x.ContentCategoryNews).ThenInclude(x => x.ContentCategories).Include(x => x.PeopleMessages).Skip(excludeRecord).Take(pageSize).ToList();
        }

        public IList<Vehicle> GetVehiclesByName(string find, int excludeRecord = 0, int pageSize = int.MaxValue){
            return _ctx.Vehicles.Include(x => x.VehiclesMappings).ThenInclude(x => x.Brands).Include(x => x.ContentCategoryNews).ThenInclude(x => x.ContentCategories).Where(x => EF.Functions.Like(x.Model, string.Concat("%", find, "%")) || EF.Functions.Like(x.VehiclesMappings.FirstOrDefault().Brands.BrandName, string.Concat("%", find, "%"))  ).Skip(excludeRecord).Take(pageSize).ToList();   
        }

        public Vehicle GetVehicleById(int Id){
            return _ctx.Vehicles.Include(x => x.VehiclesMappings).ThenInclude(x => x.Brands).Include(x => x.VehiclesMappings).ThenInclude(x => x.Supplies).Include(x => x.ContentCategoryNews).ThenInclude(x => x.ContentCategories).FirstOrDefault(x => x.Id == Id);
        }
        
        public IList<VehiclePowerSupply> GetVehiclePowerSupply(){
            return _ctx.VehiclePowerSupplies.ToList();
        }

        public VehiclePowerSupply GetVehiclePowerSupplyById(int Id){
            return _ctx.VehiclePowerSupplies.FirstOrDefault(x => x.Id == Id);
        }

        public IList<ContentCategory> GetVehicleCategory(){
            return _ctx.ContentCategories.OrderBy(x => x.Priority).ToList();
        }
        
        public IList<VehiclesImagesMapping> GetVehiclesImages(int VehicleId) {
            return _ctx.VehiclesImagesMappings.Include(x => x.Image).Where(x => x.Vehicle.Id == VehicleId).OrderBy(x => x.Image.Priority).ToList();
        }

        public VehiclesImage GetVehiclesImagesById(int Id){
            return _ctx.VehiclesImages.Include(x => x.VehiclesImages).ThenInclude(x => x.Vehicle).FirstOrDefault(x => x.Id == Id);
        }

        
        public SeoIndex GetContentNewsSeoIndexById(int Id){
            return _ctx.SeoIndex.FirstOrDefault(x => x.Id == Id);
        }

        public IList<PeopleMessage> GetVehicleContact(int vehicleId){
            return _ctx.PeopleMessages.Include(x => x.People).Where(x => x.Vehicle.Id == vehicleId).ToList();
        }

        public void InsertFirstStepVehicle(Vehicle model){
            _ctx.Vehicles.Add(model);
            _ctx.SaveChanges();
        }

        public void UpdateFirstStepVehicle(Vehicle model){
            var entityCar = _ctx.Vehicles.Find(model.Id);

            entityCar.Model = model.Model;
            entityCar.Description = model.Description;
            entityCar.Cv = model.Cv;
            entityCar.Kw = model.Kw;
            entityCar.Bookable = model.Bookable;

            _ctx.SaveChanges();
        }

        public void UpdateVehicleStep(Vehicle model){
            var entityVehicle = _ctx.Vehicles.Find(model.Id);
            entityVehicle.FirstStep = model.FirstStep;
            entityVehicle.SecondStep = model.SecondStep;
            entityVehicle.ThirdStep = model.ThirdStep;

            _ctx.SaveChanges();
        }

        public void UpdateVehicleSEO(Vehicle model){
            var entityVehicle = _ctx.Vehicles.Find(model.Id);
            entityVehicle.FirstStep = model.FirstStep;
            entityVehicle.SecondStep = model.SecondStep;
            entityVehicle.ThirdStep = model.ThirdStep;
            entityVehicle.PermaLink = model.PermaLink;
            entityVehicle.MetaTitle = model.MetaTitle;
            entityVehicle.MetaDescription = model.MetaDescription;
            entityVehicle.SeoIndexRef = model.SeoIndexRef;

            _ctx.SaveChanges();
        }

        public void InsertVehicleCategory(ContentCategoryNews model){
            _ctx.ContentCategoryNews.Add(model);
            _ctx.SaveChanges();

        }

        public void DeleteVehicleCategory(int VehicleId){
            var categoryVehicles = _ctx.ContentCategoryNews.Where(x => x.Vehicle.Id == VehicleId);
            _ctx.ContentCategoryNews.RemoveRange(categoryVehicles);
            _ctx.SaveChanges();
        }

        public void InsertVehicleMapping(VehiclesMapping model){
            _ctx.VehiclesMappings.Add(model);
            _ctx.SaveChanges();
        }

        public void DeleteVehicleMapping(int VehicleId){
            var vehicleMapping = _ctx.VehiclesMappings.Where(x => x.Vehicles.Id == VehicleId);
            _ctx.VehiclesMappings.RemoveRange(vehicleMapping);
            _ctx.SaveChanges();
        }

        public void InsertVehicleImage(VehiclesImage model){
            _ctx.VehiclesImages.Add(model);
            _ctx.SaveChanges();
        }

        public void UpdateVehicleImage(VehiclesImage model){
            var entityVehicleImage = _ctx.VehiclesImages.Find(model.Id);
            entityVehicleImage.Name = model.Name;
            entityVehicleImage.Description = model.Description;
            entityVehicleImage.Priority = model.Priority;

            _ctx.SaveChanges();
        }

        public void DeleteVehicleImage(int ImageId){
            var vehicleImageMapping = _ctx.VehiclesImagesMappings.Where(x => x.Image.Id == ImageId);
            _ctx.VehiclesImagesMappings.RemoveRange(vehicleImageMapping);

            var vehicleImage = _ctx.VehiclesImages.Where(x => x.Id == ImageId);
            _ctx.VehiclesImages.RemoveRange(vehicleImage);
            _ctx.SaveChanges();
        }

        public void InsertVehicleMapping(VehiclesImagesMapping model){
            _ctx.VehiclesImagesMappings.Add(model);
            _ctx.SaveChanges();
        }

        
    }
}