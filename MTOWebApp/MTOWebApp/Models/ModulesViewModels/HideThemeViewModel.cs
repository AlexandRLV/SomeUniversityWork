using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTOWebApp.Models.ModulesViewModels
{
    public class HideThemeViewModel
    {
        public int ModuleId { get; set; }
        public List<ApplicationUser> AllUsers { get; set; }
        public List<string> HiddenUsers { get; set; }
    }
}
