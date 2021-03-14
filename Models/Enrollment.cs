namespace ContosoUniversity.Models
{
    public enum Grade 
    {
        A, B, C, D, F
    }

    // tinfo200:[2021-03-12-sarlech-dykstra2] -- create a Enrollment class, 
    // which will late be used by the EF -- using SchoolContext which inherits from DbContext -- to create a table of the same name.
    // The class stores enrollment information (Grade, stuent id, course id, enrollment id) as properties,
    // but also contains navigation properties (" public Student Student { get; set; }" and " public Course Course { get; set; }").
    // Navigation properties are used to help the EF figure out the relationship between entity set (tables).
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public Grade? Grade { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}