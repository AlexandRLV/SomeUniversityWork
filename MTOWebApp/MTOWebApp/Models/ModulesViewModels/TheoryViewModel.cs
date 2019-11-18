using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MTOWebApp.Models.ModulesViewModels
{
    public class TheoryViewModel
    {
        [Required]
        [Display(Name = "Название модуля")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Описание модуля")]
        public string Description { get; set; }

        public int Paragraphs { get; set; }
        public int Id { get; set; }
        public int? TestId { get; set; }
    }
}
