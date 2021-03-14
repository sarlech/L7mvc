using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// tinfo200:[2021-03-12-sarlech-dykstra2] --  create a Student class, which will later be used by the EF to create
// a table of the same name
// The class stores information related to a student (name, id, enrollment date) as properties,
// but also contains "navigation" property (" public ICollection<Enrollment> Enrollments { get; set; }").
// Navigation property is used to help the EF build the relation between entity sets (tables).

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }


    }
}
