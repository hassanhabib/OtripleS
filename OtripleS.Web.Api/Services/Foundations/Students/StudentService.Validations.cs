// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Models.Students.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Students
{
    public partial class StudentService
    {
        private void ValidateStudentOnRegister(Student student)
        {
            ValidateStudent(student);

            Validate(
                (Rule: IsInvalidX(student.Id), Parameter: nameof(Student.Id)),
                (Rule: IsInvalidX(student.UserId), Parameter: nameof(Student.UserId)),
                (Rule: IsInvalidX(student.IdentityNumber), Parameter: nameof(Student.IdentityNumber)),
                (Rule: IsInvalidX(student.FirstName), Parameter: nameof(Student.FirstName)),
                (Rule: IsInvalidX(student.BirthDate), Parameter: nameof(Student.BirthDate)),
                (Rule: IsInvalidX(student.CreatedBy), Parameter: nameof(Student.CreatedBy)),
                (Rule: IsInvalidX(student.UpdatedBy), Parameter: nameof(Student.UpdatedBy)),
                (Rule: IsInvalidX(student.CreatedDate), Parameter: nameof(Student.CreatedDate)),
                (Rule: IsInvalidX(student.UpdatedDate), Parameter: nameof(Student.UpdatedDate)),
                (Rule: IsNotRecent(student.CreatedDate), Parameter: nameof(Student.CreatedDate)),
                (Rule: IsNotSame(
                        firstId: student.UpdatedBy,
                        secondId: student.CreatedBy,
                        secondIdName: nameof(Student.CreatedBy)),
                    Parameter: nameof(Student.UpdatedBy)),
                (Rule: IsNotSame(
                        firstDate: student.UpdatedDate,
                        secondDate: student.CreatedDate,
                        secondDateName: nameof(Student.CreatedDate)),
                    Parameter: nameof(Student.UpdatedDate))
            );
        }

        private static dynamic IsInvalidX(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalidX(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalidX(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset dateTimeOffset) => new
        {
            Condition = IsDateNotRecent(dateTimeOffset),
            Message = "Date is not recent"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidStudentException = new InvalidStudentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidStudentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidStudentException.ThrowIfContainsErrors();
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

        private void ValidateStudentOnModify(Student student)
        {
            ValidateStudent(student);
            Validate(
                (Rule: IsInvalidX(student.Id), Parameter: nameof(Student.Id)),
                (Rule: IsInvalidX(student.UserId), Parameter: nameof(Student.UserId)),
                (Rule: IsInvalidX(student.IdentityNumber), Parameter: nameof(Student.IdentityNumber)),
                (Rule: IsInvalidX(student.FirstName), Parameter: nameof(Student.FirstName)),
                (Rule: IsInvalidX(student.BirthDate), Parameter: nameof(Student.BirthDate)),
                (Rule: IsInvalidX(student.CreatedBy), Parameter: nameof(Student.CreatedBy)),
                (Rule: IsInvalidX(student.UpdatedBy), Parameter: nameof(Student.UpdatedBy)),
                (Rule: IsInvalidX(student.CreatedDate), Parameter: nameof(Student.CreatedDate)),
                (Rule: IsInvalidX(student.UpdatedDate), Parameter: nameof(Student.UpdatedDate)),
                (Rule: IsNotRecent(student.UpdatedDate), Parameter: nameof(Student.UpdatedDate)),
                (Rule: IsSame(
                        firstDate: student.UpdatedDate,
                        secondDate: student.CreatedDate,
                        secondDateName: nameof(Student.CreatedDate)),
                    Parameter: nameof(Student.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageStudentOnModify(Student inputStudent, Student storageStudent)
        {
            Validate(
                (Rule: IsNotSame(
                        firstDate: inputStudent.CreatedDate,
                        secondDate: storageStudent.CreatedDate,
                        secondDateName: nameof(Student.CreatedDate)),
                    Parameter: nameof(Student.CreatedDate)),
                (Rule: IsSame(
                        firstDate: inputStudent.UpdatedDate,
                        secondDate: storageStudent.UpdatedDate,
                        secondDateName: nameof(Student.UpdatedDate)),
                    Parameter: nameof(Student.UpdatedDate)),
                (Rule: IsNotSame(
                        firstId: inputStudent.CreatedBy,
                        secondId: storageStudent.CreatedBy,
                        secondIdName: nameof(Student.CreatedBy)),
                    Parameter: nameof(Student.CreatedBy))
            );
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
        private static void ValidateStudent(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }
        }

        private void ValidateStorageStudents(IQueryable<Student> storageStudents)
        {
            if (!storageStudents.Any())
            {
                this.loggingBroker.LogWarning("No students found in storage.");
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;
    }
}