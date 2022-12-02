using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Entities
{
    public class BirthProbability
    {
        public int Kor { get; set; }
        public int gyermekek { get; set; }
        public double valsz { get; set; }
        public object P { get; internal set; }
    }
}
