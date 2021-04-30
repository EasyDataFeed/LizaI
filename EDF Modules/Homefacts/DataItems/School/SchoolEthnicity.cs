using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homefacts.DataItems.School
{
    public  class SchoolEthnicity
    {
        public int Id { get; set; }
        public string Asian { get; set; }
        public string AfricanAmerican { get; set; }
        public string Hispanic { get; set; }
        public string HawaiianOrPacificIslander { get; set; }
        public string AmericanIndianOrAlaskan { get; set; }
        public string White { get; set; }
        public string TwoOrMoreRaces { get; set; }
    }
}
