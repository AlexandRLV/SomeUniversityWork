﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppMTO.Data
{
    public class TestScore
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationStudent { get; set; }
        public TestModule TestModule { get; set; }
        public DateTime TestDate { get; set; }
        public int Score { get; set; }
    }
}
