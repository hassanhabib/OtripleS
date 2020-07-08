// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;

namespace OtripleS.Web.Api.Services.Teachers
{
    public partial class TeacherService
    {
        private void ValidateTeacherId(Guid teacherId)
        {
            if (teacherId == default)
            {
                throw new InvalidTeacherInputException(
                    parameterName: nameof(Teacher.Id),
                    parameterValue: teacherId);
            }
        }

        private void ValidateStorageTeacher(Teacher maybeTeacher, Guid teacherId)
        {
            if (maybeTeacher is null)
            {
                throw new NotFoundTeacherException(teacherId);
            }
        }

        private void ValidateTeacherOnModify(Teacher teacher)
        {
            ValidateTeacher(teacher);
            ValidateTeacherId(teacher.Id);
            ValidateTeacherIds(teacher);
            ValidateTeacherStrings(teacher);
            ValidateTeacherDates(teacher);
            ValidateDatesAreNotSame(teacher);
            ValidateUpdatedDateIsRecent(teacher);
        }

        public void ValidateAgainstStorageTeacherOnModify(Teacher inputTeacher, Teacher storageTeacher)
        {
            switch (inputTeacher)
            {
                case { } when inputTeacher.CreatedDate != storageTeacher.CreatedDate:
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(Teacher.CreatedDate),
                        parameterValue: inputTeacher.CreatedDate);

                case { } when inputTeacher.CreatedBy != storageTeacher.CreatedBy:
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(Teacher.CreatedBy),
                        parameterValue: inputTeacher.CreatedBy);

                case { } when inputTeacher.UpdatedDate == storageTeacher.UpdatedDate:
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(Teacher.UpdatedDate),
                        parameterValue: inputTeacher.UpdatedDate);
            }
        }

        private void ValidateTeacher(Teacher teacher)
        {
            if(teacher is null)
            {
                throw new NullTeacherException();
            }
        }

        private void ValidateTeacherIds(Teacher teacher)
        {
            switch (teacher)
            {
                case { } when IsInvalid(teacher.CreatedBy):
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(Teacher.CreatedBy),
                        parameterValue: teacher.CreatedBy);

                case { } when IsInvalid(teacher.UpdatedBy):
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(Teacher.UpdatedBy),
                        parameterValue: teacher.UpdatedBy);
            }
        }

        private void ValidateTeacherStrings(Teacher teacher)
        {
            switch (teacher)
            {
                case { } when IsInvalid(teacher.UserId):
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(teacher.UserId),
                        parameterValue: teacher.UserId);

                case { } when IsInvalid(teacher.EmployeeNumber):
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(teacher.EmployeeNumber),
                        parameterValue: teacher.EmployeeNumber);

                case { } when IsInvalid(teacher.FirstName):
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(teacher.FirstName),
                        parameterValue: teacher.FirstName);
            }
        }

        private void ValidateTeacherDates(Teacher teacher)
        {
            switch (teacher)
            {
                case { } when teacher.CreatedDate == default:
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(Teacher.CreatedDate),
                        parameterValue: teacher.CreatedDate);

                case { } when teacher.UpdatedDate == default:
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(Teacher.UpdatedDate),
                        parameterValue: teacher.UpdatedDate);
            }
        }

        private void ValidateDatesAreNotSame(Teacher teacher)
        {
            if (teacher.CreatedDate == teacher.UpdatedDate)
            {
                throw new InvalidTeacherInputException(
                    parameterName: nameof(Teacher.CreatedDate),
                    parameterValue: teacher.CreatedDate);
            }
        }

        private void ValidateUpdatedDateIsRecent(Teacher teacher)
        {
            if (IsDateNotRecent(teacher.UpdatedDate))
            {
                throw new InvalidTeacherInputException(
                    parameterName: nameof(Teacher.UpdatedDate),
                    parameterValue: teacher.UpdatedDate);
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;
        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
