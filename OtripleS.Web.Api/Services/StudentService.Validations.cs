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
