using System;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Models.Students.Exceptions;

namespace OtripleS.Web.Api.Services
{
    public partial class StudentService
    {
        private void ValidateStudentId(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                throw new InvalidStudentInputException(
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

        private void ValidateStudentOnCreate(Student student)
        {
            ValidateStudent(student);
            ValidateStudentId(student.Id);
            ValidateStudentRequiredData(student);
        }

        private void ValidateStudentRequiredData(Student student)
        {
            switch(student)
            {
                case { } when student.BirthDate == default:
                    throw new InvalidStudentInputException(
                        parameterName: nameof(Student.BirthDate),
                        parameterValue: student.BirthDate);

                case { } when student.CreatedBy == default:
                    throw new InvalidStudentInputException(
                        parameterName: nameof(Student.CreatedBy),
                        parameterValue: student.CreatedBy);
            }
        }

        private void ValidateStudent(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }
        }
    }
}
