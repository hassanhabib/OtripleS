using OtripleS.Web.Api.Models.Students;
using System;

namespace OtripleS.Web.Api.Services
{
    public partial class StudentService
    {
        public void ValidateStudent(Student student)
        {
            ValidateStudentIsNotNull(student);
            ValidateStudentId(student.Id);
            ValidateStudentName(student);
        }

        private void ValidateStudentIsNotNull(Student student)
        {
            if (student is null)
            {
                throw new ArgumentNullException(nameof(student));
            }
        }

        private static void ValidateStudentName(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FirstName))
            {
                throw new ArgumentException("The student first name is required.", nameof(student.FirstName));
            }

            if (string.IsNullOrWhiteSpace(student.LastName))
            {
                throw new ArgumentException("The student last name is required.", nameof(student.LastName));
            }
        }

        private static void ValidateStudentId(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                throw new ArgumentException("The student id is required.", nameof(studentId));
            }
        }
    }
}
