using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SELF
{
    class doviz
    {
        public int Birim { get; set; }
        public string DovizAdi { get; set; }
        public double Alis { get; set; }
        public double Satis { get; set; }

        public override string ToString()
        {
            return DovizAdi;
        }
    }
}

