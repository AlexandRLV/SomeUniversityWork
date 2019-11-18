using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebAppMTO.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Task { get; set; }
        public string CorrectAnswer { get; set; }

        public TestModule TestModule { get; set; }
        public TheoryModule TheoryModule { get; set; }
    }
}
