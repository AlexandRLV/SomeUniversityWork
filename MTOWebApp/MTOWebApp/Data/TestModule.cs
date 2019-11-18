using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTOWebApp.Data
{
    public class TestModule
    {
        public int Id { get; set; }

        public TheoryModule TheoryModule { get; set; }
    }
}