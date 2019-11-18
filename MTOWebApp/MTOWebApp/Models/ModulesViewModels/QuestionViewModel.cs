using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MTOWebApp.Models.ModulesViewModels
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public string Type { get; set; }
        
        public List<string> Answers1 { get; set; }
        public List<string> Answers2 { get; set; }

        [Required]
        public int TestModuleId { get; set; }

        [Required]
        public int SerialNum { get; set; }
    }
}
