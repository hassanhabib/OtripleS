using System;
using System.Linq;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Models.Students.Exceptions;

namespace OtripleS.Web.Api.Services
{
    public partial class StudentService
    {
        public void ValidateStudent(Student student)
        {
            ValidateStudentIsNotNull(student);
            ValidateStudentId(student.Id);
            ValidateStudentName(student);
            // ValidateStudentBirthDate(student.BirthDate);
        }

        private void ValidateStudentIsNotNull(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }
        }

        private static void ValidateStudentBirthDate(DateTimeOffset birthDate)
        {
            var studentMaxAge = 100;
            var studentMinAge = 2;
            var today = DateTime.Today;

            if (birthDate > today.AddYears(studentMaxAge) || birthDate < today.AddYears(-1 * studentMinAge))
            {
                throw new InvalidStudentException(
                    parameterName: nameof(Student.BirthDate),
                    parameterValue: birthDate);
            }
        }

        private static void ValidateStudentName(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FirstName))
            {
                throw new InvalidStudentException(
                    parameterName: nameof(Student.FirstName),
                    parameterValue: student.FirstName);
            }
        }

        private static void ValidateStudentId(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                throw new InvalidStudentException(
                    parameterName: nameof(Student.Id),
                    parameterValue: studentId);
            }
        }

        private static void ValidateStorageStudent(Student storageStudent, Guid studentId)
        {
            if (storageStudent == null)
            {
                throw new NotFoundStudentException(studentId);
            }
        }

        private void ValidateStorageStudents(IQueryable<Student> storageStudents)
        {
            if (storageStudents.Count() == 0)
            {
                this.loggingBroker.LogWarning("Students storage is empty.");
            }
        }
    }
}