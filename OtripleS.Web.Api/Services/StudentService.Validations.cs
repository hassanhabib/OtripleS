using System;
using System.Runtime.CompilerServices;
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
            ValidateCreatedSignature(student);
            ValidateDates(student);
        }

        private void ValidateStudentOnModify(Student student)
        {
            ValidateStudent(student);
            ValidateStudentId(student.Id);
        }

        private void ValidateDates(Student student)
        {
            if (IsDateNotRecent(student.CreatedDate))
            {
                throw new InvalidStudentInputException(
                    parameterName: nameof(student.CreatedDate),
                    parameterValue: student.CreatedDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int validMinuteVariation = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > validMinuteVariation;
        }

        private void ValidateCreatedSignature(Student student)
        {
            if (student.CreatedBy != student.UpdatedBy)
            {
                throw new InvalidStudentInputException(
                    parameterName: nameof(Student.UpdatedBy),
                    parameterValue: student.UpdatedBy);
            }
            else if (student.CreatedDate != student.UpdatedDate)
            {
                throw new InvalidStudentInputException(
                    parameterName: nameof(Student.UpdatedDate),
                    parameterValue: student.UpdatedDate);
            }
        }

        private void ValidateStudentRequiredData(Student student)
        {
            switch (student)
            {
                case { } when student.BirthDate == default:
                    throw new InvalidStudentInputException(
                        parameterName: nameof(Student.BirthDate),
                        parameterValue: student.BirthDate);

                case { } when student.CreatedBy == default:
                    throw new InvalidStudentInputException(
                        parameterName: nameof(Student.CreatedBy),
                        parameterValue: student.CreatedBy);

                case { } when student.CreatedDate == default:
                    throw new InvalidStudentInputException(
                        parameterName: nameof(Student.CreatedDate),
                        parameterValue: student.CreatedDate);

                case { } when student.UpdatedBy == default:
                    throw new InvalidStudentInputException(
                        parameterName: nameof(Student.UpdatedBy),
                        parameterValue: student.UpdatedBy);

                case { } when student.UpdatedDate == default:
                    throw new InvalidStudentInputException(
                        parameterName: nameof(Student.UpdatedDate),
                        parameterValue: student.UpdatedDate);
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
