using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public class ProfilingSystemMenu
    {
        public ProfilingSystemMenu(){
            ProfilingGroupSystemMenus = new HashSet<ProfilingGroupSystemMenu>();
        }
        public int Id { get; set; }
        public string CodMenu { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int Priority { get; set; } 
        public int Visible { get; set; }
        public int? MenuFatherId { get; set; }
        public int DisplayHeader { get; set; }
        public ICollection<ProfilingGroupSystemMenu> ProfilingGroupSystemMenus { get; set; } 
    }
}