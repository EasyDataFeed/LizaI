using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homefacts.DataItems.School
{
    public class SchoolCharacteristic
    {
        public int Id { get; set; }
        public string Locale { get; set; }
        public string StudentBody { get; set; }
        public int NumberOfTeachers { get; set; }
        public int DaysInSchoolYear { get; set; }
        public int HoursInSchoolDay { get; set; }
        public int ReligiousOrientation { get; set; }
        public int ReligiousAffiliation { get; set; }
        public string Religion { get; set; }
        public string CatholicAffiliation { get; set; }
        public string Association { get; set; }
    }
}
