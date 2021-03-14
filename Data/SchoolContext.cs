using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Data
{
    //tinfo200:[2021-03-12-costarec-dykstra2] --  create a SchoolContext class that inherites from DbContext. The class coordinates the functionality of the
    //EF to creates tables and relationships using the classes inside the Models folder (Student.cs, Course.cs, Enrollment.cs).
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        //tinfo200:[2021-03-04-costarec-dykstra2] --  DbSet<Type>: we create a three sets to hold the Course, Enrollments and Students objects.
        //The sets are called Entity set and their type is enclosed in the angle brackets. 
        //Properties of type DbSet<type> will be used by the EF to create tables - they basically become tables and their properties become columns in these tables.
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }


        //tinfo200:[2021-03-04-costarec-dykstra2] --  When using Entity Sets (DbSet<Type>) to create the tables, the Entity Sets names are used as table names.
        // However, since an Entity Set names are typically plural, we used the method below and rename the tables with singular names.
        // Note, the "override" keyword. This method will not be referenced anywhere in the entire project, but will be called in the dependency injection
        // when building the database.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
        }
    }
}
