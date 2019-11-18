using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTOWebApp.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Task { get; set; }
        public string Answers1 { get; set; }
        public string Answers2 { get; set; }
        public int SerialNum { get; set; }

        public TestModule TestModule { get; set; }
    }
}
