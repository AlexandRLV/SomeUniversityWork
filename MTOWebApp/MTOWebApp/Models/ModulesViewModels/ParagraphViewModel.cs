using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MTOWebApp.Models.ModulesViewModels
{
    public class ParagraphViewModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public int ModuleId { get; set; }
        public int SerialNum { get; set; }

        public IFormFile Picture { get; set; }
        public IFormFile Audio { get; set; }
        public IFormFile Video { get; set; }
    }
}
