using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


// tinfo200:[2021-03-12-sarlech-dykstra2] -- create a Course class, 
// which will later be used by the EF - using a context, in this case SchoolContext, inheriting from the DbContext class -
// to create a table of the same name.
// The class stores information about a course (credit earned, id, title of the course) as properties,
// but also contains navigation property (" public ICollection<Enrollment> Enrollments { get; set; }").
// Navigation property is used to help the EF figure out the relationship between entity sets (tables).

namespace ContosoUniversity.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
