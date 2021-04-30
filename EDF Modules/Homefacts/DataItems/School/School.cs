using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homefacts.DataItems.School
{
    public class School
    {
        public int Id { get; set; }
        public string SchoolType { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolSummary { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int IdCharacteristic { get; set; }
        public int InstitutionType { get; set; }
        public int TotalEnrollment { get; set; }
        public int InStateTuition { get; set; }
        public int IdHomefact { get; set; }
        public int IdEthnicity { get; set; }
        public int IdAllTables { get; set; }
        public int IdEnrollmentByGrade { get; set; }
        public int IdAdmission { get; set; }

        public List<SchoolAdmission> Admissions { get; set; }
        public List<SchoolAllTables> AllTables { get; set; }
        public List<SchoolCharacteristic> Characteristics { get; set; }
        public List<SchoolEnrollmentByGrade> EnrollmentByGrades { get; set; }
        public List<SchoolEthnicity> Ethnicities { get; set; }
        public List<SchoolHomefact> Homefacts { get; set; }
    }
}
