// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Models.Students.Exceptions;

namespace OtripleS.Web.Api.Services.Students
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

        private void ValidateStudentOnCreate(Student student)
        {
            switch (student)
            {
                case null:
                    throw new NullStudentException();

                case { } when IsInvalid(student.Id):
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.Id),
                        parameterValue: student.Id);

                case { } when IsInvalid(student.CreatedBy):
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.CreatedBy),
                        parameterValue: student.CreatedBy);

                case { } when IsInvalid(student.UpdatedBy):
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.UpdatedBy),
                        parameterValue: student.UpdatedBy);

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

                case { } when IsInvalid(student.BirthDate):
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.BirthDate),
                        parameterValue: student.BirthDate);

                case { } when IsNotSame(student.CreatedBy, student.UpdatedBy):
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.UpdatedBy),
                        parameterValue: student.UpdatedBy);

                case { } when IsNotSame(student.CreatedDate, student.UpdatedDate):
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.UpdatedDate),
                        parameterValue: student.UpdatedDate);

                case { } when IsDateNotRecent(student.CreatedDate):
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.CreatedDate),
                        parameterValue: student.CreatedDate);
            }
        }

        private void ValidateStudentOnModify(Student student)
        {
            ValidateStudent(student);
            ValidateStudentId(student.Id);
            ValidateStudentStrings(student);
            ValidateStudentDates(student);
            ValidateStudentIds(student);
            ValidateDatesAreNotSame(student);
            ValidateUpdatedDateIsRecent(student);
        }

        public void ValidateAginstStorageStudentOnModify(Student inputStudent, Student storageStudent)
        {
            switch (inputStudent)
            {
                case { } when inputStudent.CreatedDate != storageStudent.CreatedDate:
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.CreatedDate),
                        parameterValue: inputStudent.CreatedDate);

                case { } when inputStudent.CreatedBy != storageStudent.CreatedBy:
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.CreatedBy),
                        parameterValue: inputStudent.CreatedBy);

                case { } when inputStudent.UpdatedDate == storageStudent.UpdatedDate:
                    throw new InvalidStudentException(
                        parameterName: nameof(Student.UpdatedDate),
                        parameterValue: inputStudent.UpdatedDate);
            }
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

        private void ValidateDatesAreNotSame(Student student)
        {
            if (student.CreatedDate == student.UpdatedDate)
            {
                throw new InvalidStudentException(
                    parameterName: nameof(Student.CreatedDate),
                    parameterValue: student.CreatedDate);
            }
        }

        private void ValidateUpdatedDateIsRecent(Student student)
        {
            if (IsDateNotRecent(student.UpdatedDate))
            {
                throw new InvalidStudentException(
                    parameterName: nameof(student.UpdatedDate),
                    parameterValue: student.UpdatedDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private static void ValidateStudentDates(Student student)
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

        private static void ValidateStudentIds(Student student)
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

        private static void ValidateStudent(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }
        }

        private void ValidateStorageStudents(IQueryable<Student> storageStudents)
        {
            if (storageStudents.Count() == 0)
            {
                this.loggingBroker.LogWarning("No students found in storage.");
            }
        }

        private static bool IsNotSame(Guid firstId, Guid secondId) => firstId != secondId;

        private static bool IsNotSame(DateTimeOffset firstDate, DateTimeOffset secondDate) =>
            firstDate != secondDate;

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;
        private static bool IsInvalid(DateTimeOffset date) => date == default;
    }
}
