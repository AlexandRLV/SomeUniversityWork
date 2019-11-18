using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTOWebApp.Data;

namespace MTOWebApp.Models.ModulesViewModels
{
    public class TestResultViewModel
    {
        public int Score { get; set; }

        public DateTime Date { get; set; }
        public TestModule TestModule { get; set; }
        public List<QuestionViewModel> Answers { get; set; }
    }
}
