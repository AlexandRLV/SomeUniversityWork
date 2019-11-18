using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetApp1.Models
{
    // Класс категории
    public class Category
    {
        public int Id;
        public string Name;

        public override string ToString()
        {
            return $"{Id}: {Name}";
        }
    }
}
