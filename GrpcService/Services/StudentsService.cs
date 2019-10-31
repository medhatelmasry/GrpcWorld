using Grpc.Core;
using GrpcService.Data;
using GrpcService.Models;
using GrpcService.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService.Services
{
    public class StudentsService : RemoteStudent.RemoteStudentBase
    {
        private readonly ILogger<StudentsService> _logger;
        private readonly SchoolDbContext _context;

        public StudentsService(ILogger<StudentsService> logger, SchoolDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public override Task<StudentModel> GetStudentInfo(StudentLookupModel request, ServerCallContext context)
        {
            StudentModel output = new StudentModel();

            var student = _context.Students.Find(request.StudentId);

            _logger.LogInformation("Sending Student response");

            if (student != null)
            {
                output.StudentId = student.StudentId;
                output.FirstName = student.FirstName;
                output.LastName = student.LastName;
                output.School = student.School;
            }

            return Task.FromResult(output);
        }
        public override Task<Reply> InsertStudent(StudentModel request, ServerCallContext context)
        {
            var s = _context.Students.Find(request.StudentId);

            if (s != null)
            {
                return Task.FromResult(
                  new Reply()
                  {
                      Result = $"Student {request.FirstName} {request.LastName} already exists.",
                      IsOk = false
                  }
                );
            }

            Student student = new Student()
            {
                StudentId = request.StudentId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                School = request.School,
            };

            _logger.LogInformation("Insert student");

            try
            {
                _context.Students.Add(student);
                var returnVal = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Task.FromResult(
               new Reply()
               {
                   Result = $"Student {request.FirstName} {request.LastName}  was successfully inserted.",
                   IsOk = true
               }
            );
        }

        public override Task<Reply> UpdateStudent(StudentModel request, ServerCallContext context)
        {
            var s = _context.Students.Find(request.StudentId);

            if (s == null)
            {
                return Task.FromResult(
                  new Reply()
                  {
                      Result = $"Student {request.FirstName} {request.LastName} cannot be found.",
                      IsOk = false
                  }
                );
            }

            s.FirstName = request.FirstName;
            s.LastName = request.LastName;
            s.School = request.School;

            _logger.LogInformation("Update student");

            try
            {
                var returnVal = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Task.FromResult(
               new Reply()
               {
                   Result = $"Student {request.FirstName} {request.LastName} was successfully updated.",
                   IsOk = true
               }
            );
        }

        public override Task<Reply> DeleteStudent(StudentLookupModel request, ServerCallContext context)
        {
            var s = _context.Students.Find(request.StudentId);

            if (s == null)
            {
                return Task.FromResult(
                  new Reply()
                  {
                      Result = $"Student with ID {request.StudentId} cannot be found.",
                      IsOk = false
                  }
                );
            }

            _logger.LogInformation("Delete Student");

            try
            {
                _context.Students.Remove(s);
                var returnVal = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Task.FromResult(
               new Reply()
               {
                   Result = $"Student with ID {request.StudentId} was successfully deleted.",
                   IsOk = true
               }
            );
        }

        public override Task<StudentList> RetrieveAllStudents(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Retrieving all students");

            StudentList list = new StudentList();

            try
            {
                List<StudentModel> studentList = new List<StudentModel>();

                var students = _context.Students.ToList();

                foreach (var c in students)
                {
                    studentList.Add(new StudentModel()
                    {
                        StudentId = c.StudentId,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        School = c.School,
                    });
                }

                list.Items.AddRange(studentList);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Task.FromResult(list);
        }
    }
}
