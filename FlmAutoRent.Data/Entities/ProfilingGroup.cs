using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public partial class ProfilingGroup
    {
        public ProfilingGroup(){
            ProfilingOperatorGroups = new HashSet<ProfilingOperatorGroup>();
            ProfilingGroupSystemMenus = new HashSet<ProfilingGroupSystemMenu>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Data { get; set; }

        public ICollection<ProfilingOperatorGroup> ProfilingOperatorGroups { get; set; } 
        public ICollection<ProfilingGroupSystemMenu> ProfilingGroupSystemMenus { get; set; } 
    }
}