using OtripleS.Web.Api.Models.Students;
using SchoolEM.Models.Students.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services
{
    public partial class StudentService
    {
        public void ValidateStudent(Student student)
        {
            ValidateStudentIsNotNull(student);
            ValidateStudentId(student.Id);
            ValidateStudentFirstName(student);
            ValidateStudentMiddleName(student);
            ValidateStudentLastName(student);
        }

        private void ValidateStudentIsNotNull(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }
        }

        private static void ValidateStudentFirstName(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FirstName))
            {
                throw new InvalidStudentException(
                    parameterName: nameof(Student.FirstName),
                    parameterValue: student.FirstName);
            }
        }

        private static void ValidateStudentMiddleName(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.MiddleName))
            {
                throw new InvalidStudentException(
                    parameterName: nameof(Student.MiddleName),
                    parameterValue: student.MiddleName);
            }
        }

        private static void ValidateStudentLastName(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.LastName))
            {
                throw new InvalidStudentException(
                    parameterName: nameof(Student.LastName),
                    parameterValue: student.LastName);
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
