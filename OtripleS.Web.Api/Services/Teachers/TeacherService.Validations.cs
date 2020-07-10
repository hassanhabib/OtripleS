// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;

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

        private void ValidateStorageTeachers(IQueryable<Teacher> storageTeachers)
        {
            if (storageTeachers.Count() == 0)
            {
                this.loggingBroker.LogWarning("No teachers found in storage.");
            }
        }

        private void ValidateTeacherOnCreate(Teacher teacher)
        {
            ValidateTeacher(teacher);
            ValidateTeacherId(teacher.Id);
            ValidateTeacherStrings(teacher);
            ValidateTeacherDates(teacher);
            ValidateTeacherIds(teacher);
            ValidateCreatedDateIsNotRecent(teacher);
        }

        private void ValidateCreatedDateIsNotRecent(Teacher teacher)
        {
            if (IsDateNotRecent(teacher.CreatedDate))
            {
                throw new InvalidTeacherInputException(
                    parameterName: nameof(teacher.CreatedDate),
                    parameterValue: teacher.CreatedDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private void ValidateTeacherDates(Teacher teacher)
        {
            switch (teacher)
            {
                case { } when teacher.CreatedDate == default:
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(Teacher.CreatedDate),
                        parameterValue: teacher.CreatedDate);

                case { } when teacher.UpdatedDate == default || teacher.CreatedDate != teacher.UpdatedDate:
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(Teacher.UpdatedDate),
                        parameterValue: teacher.UpdatedDate);
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

                case { } when IsInvalid(teacher.UpdatedBy) || teacher.CreatedBy != teacher.UpdatedBy:
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(Teacher.UpdatedBy),
                        parameterValue: teacher.UpdatedBy);
            }
        }

        private void ValidateTeacher(Teacher teacher)
        {
            if (teacher == default)
            {
                throw new NullTeacherException();
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

                case { } when IsInvalid(teacher.LastName):
                    throw new InvalidTeacherInputException(
                        parameterName: nameof(teacher.LastName),
                        parameterValue: teacher.LastName);
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;
    }
}
