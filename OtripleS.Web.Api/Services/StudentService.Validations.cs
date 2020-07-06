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

        private void ValidateStudentOnCreate(Student student)
        {
            ValidateStudent(student);
            ValidateStudentId(student.Id);
            ValidateStudentIds(student);
            ValidateStudentDates(student);
            ValidateCreatedSignature(student);
            ValidateDates(student);
        }

        private void ValidateStudentOnModify(Student student)
        {
            ValidateStudent(student);
            ValidateStudentId(student.Id);
            ValidateStudentStrings(student);
            ValidateStudentDates(student);
            ValidateStudentIds(student);
        }

        private void ValidateStudentStrings(Student student)
        {
            switch (student)
            {
                case { } when IsInvalid(student.UserId):
                    throw new InvalidStudentException(
                        parameterName: nameof(student.UserId),
                        parameterValue: student.UserId);

                case { } when IsInvalid(student.IdentityNumber):
                    throw new InvalidStudentException(
                        parameterName: nameof(student.IdentityNumber),
                        parameterValue: student.IdentityNumber);

                case { } when IsInvalid(student.FirstName):
                    throw new InvalidStudentException(
                        parameterName: nameof(student.FirstName),
                        parameterValue: student.FirstName);
            }
        }

        private void ValidateDates(Student student)
        {
            if (IsDateNotRecent(student.CreatedDate))
            {
                throw new InvalidStudentException(
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
                throw new InvalidStudentException(
                    parameterName: nameof(Student.UpdatedBy),
                    parameterValue: student.UpdatedBy);
            }
            else if (student.CreatedDate != student.UpdatedDate)
            {
                throw new InvalidStudentException(
                    parameterName: nameof(Student.UpdatedDate),
                    parameterValue: student.UpdatedDate);
            }
        }

        private void ValidateStudentDates(Student student)
        {
            switch (student)
            {
                case { } when student.BirthDate == default:
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.BirthDate),
                        parameterValue: student.BirthDate);

                case { } when student.CreatedDate == default:
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.CreatedDate),
                        parameterValue: student.CreatedDate);

                case { } when student.UpdatedDate == default:
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.UpdatedDate),
                        parameterValue: student.UpdatedDate);
            }
        }

        private void ValidateStudentIds(Student student)
        {
            switch (student)
            {
                case { } when IsInvalid(student.CreatedBy):
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.CreatedBy),
                        parameterValue: student.CreatedBy);

                case { } when IsInvalid(student.UpdatedBy):
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.UpdatedBy),
                        parameterValue: student.UpdatedBy);
            }
        }

        private void ValidateStudent(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;
    }
}
