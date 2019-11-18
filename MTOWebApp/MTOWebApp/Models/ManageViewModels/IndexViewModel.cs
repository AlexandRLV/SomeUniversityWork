using MTOWebApp.Data;
using MTOWebApp.Models.ModulesViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MTOWebApp.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public string StatusMessage { get; set; }

        public TestScore LastTest { get; set; }
    }
}
