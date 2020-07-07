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

        private void ValidateStudent(Student student)
		{
            ValidateStudentId(student.Id);
            ValidateStudentName(student);
            ValidateBirthDate(student.BirthDate);
		}

		private void ValidateBirthDate(DateTimeOffset birthDate)
		{
            if (birthDate >= DateTime.UtcNow || birthDate == default(DateTimeOffset))
			{
                throw new InvalidStudentInputException(
                    parameterName: nameof(Student.BirthDate),
                    parameterValue: birthDate);
            }
        }

		private static void ValidateStudentName(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FirstName))
            {
                throw new InvalidStudentInputException(
                    parameterName: nameof(Student.FirstName),
                    parameterValue: student.FirstName);
            }

            if (string.IsNullOrWhiteSpace(student.LastName))
            {
                throw new InvalidStudentInputException(
                    parameterName: nameof(Student.LastName),
                    parameterValue: student.LastName);
            }
        }

        private static void ValidateStorageStudent(Student storageStudent, Guid studentId)
        {
            if (storageStudent == null)
            {
                throw new NotFoundStudentException(studentId);
            }
        }
    }
}
