using System.Collections.Generic;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class MenuViewModel 
    {
        public IList<ProfilingSystemMenu> Menu { get; set; } = new List<ProfilingSystemMenu>();
        public IList<MenuDisplayViewModel> MenuHeader { get; set; } = new List<MenuDisplayViewModel>();
        public IList<MenuDisplayViewModel> MenuAside { get; set; } = new List<MenuDisplayViewModel>();
    }

    public class MenuDisplayViewModel : ProfilingSystemMenu
    {
        public string Active { get; set; }
    }
}