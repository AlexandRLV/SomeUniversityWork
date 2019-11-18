using MTOWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTOWebApp.Data
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public string Type { get; set; }
        public string Answers1 { get; set; }
        public string Answers2 { get; set; }
        public string StudentAnswer { get; set; }

        public ApplicationUser Student { get; set; }
        public Question Question { get; set; }
    }
}
