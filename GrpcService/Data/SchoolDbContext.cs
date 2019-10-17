using GrpcService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Student>().HasData(
              GetStudents()
            );
        }

        public DbSet<Student> Students { get; set; }

        private static List<Student> GetStudents()
        {
            List<Student> students = new List<Student>() {
              new Student() {    // 1
                StudentId=11,
                FirstName="Ann",
                LastName="Fox",
                School="Nursing",
              },
              new Student() {    // 2
                StudentId=22,
                FirstName="Sam",
                LastName="Doe",
                School="Mining",
              },
              new Student() {    // 3
                StudentId=33,
                FirstName="Sue",
                LastName="Cox",
                School="Business",
              },
              new Student() {    // 4
                StudentId=44,
                FirstName="Tim",
                LastName="Lee",
                School="Computing",
              },
              new Student() {    // 5
                StudentId=55,
                FirstName="Jan",
                LastName="Ray",
                School="Computing",
              },
            };

            return students;
        }
    }

}
