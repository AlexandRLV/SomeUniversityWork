using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppMTO.Data
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public string CorrectAnswer { get; set; }
        public string StudentAnswer { get; set; }

        public ApplicationUser Student { get; set; }
        public Question Question { get; set; }
    }
}
