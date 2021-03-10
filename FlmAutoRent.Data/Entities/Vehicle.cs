using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public class Vehicle
    {
        public Vehicle(){
            VehiclesImages = new HashSet<VehiclesImagesMapping>();
            VehiclesMappings = new HashSet<VehiclesMapping>();
            PeopleMessages = new HashSet<PeopleMessage>();
            ContentCategoryNews = new HashSet<ContentCategoryNews>();
        }

        public int Id { get; set; }
        public string Model { get; set; }
        public string Cv { get; set; }
        public string Kw { get; set; }
        public string Description { get; set; }

        public ICollection<VehiclesImagesMapping> VehiclesImages { get; set; }
        public ICollection<VehiclesMapping> VehiclesMappings { get; set; }
        public ICollection<ContentCategoryNews> ContentCategoryNews { get; set; }
        public ICollection<PeopleMessage> PeopleMessages { get; set; }
        public string PermaLink { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }

        public int SeoIndexRef { get; set;}
        public SeoIndex SeoIndex { get; set; }

        public DateTime OperatorData { get; set; }
        public int IDOperator { get; set; }

        public bool FirstStep { get; set; }
        public bool SecondStep { get; set; }
        public bool ThirdStep { get; set; }
        public bool Bookable { get; set; } 
        public bool DisplayHp { get; set; }
        public int Priority { get; set; }
    }
}