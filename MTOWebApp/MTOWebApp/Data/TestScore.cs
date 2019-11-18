using MTOWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTOWebApp.Data
{
    public class TestScore
    {
        public int Id { get; set; }
        public int Score { get; set; }

        public ApplicationUser ApplicationStudent { get; set; }
        public TestModule TestModule { get; set; }
        public DateTime TestDate { get; set; }
    }
}
