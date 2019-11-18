using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTOWebApp.Data
{
    public class Paragraph
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SerialNum { get; set; }
        public string PictureFileName { get; set; }
        public string VideoFileName { get; set; }
        public string SoundFileName { get; set; }
        public string AnimFileName { get; set; }

        public TheoryModule TheoryModule { get; set; }
    }
}
