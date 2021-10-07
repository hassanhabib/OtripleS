// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Classrooms
{
    public partial class ClassroomService
    {
        private void ValidateClassroomOnCreate(Classroom classroom)
        {
            ValidateClassroomIsNull(classroom);
            ValidateClassroomIdIsNull(classroom.Id);
            ValidateClassroomFields(classroom);
            ValidateInvalidAuditFields(classroom);
            ValidateAuditFieldsDataOnCreate(classroom);
        }

        private void ValidateClassroomOnModify(Classroom classroom)
        {
            ValidateClassroomIsNull(classroom);
            ValidateClassroomIdIsNull(classroom.Id);
            ValidateClassroomFields(classroom);
            ValidateInvalidAuditFields(classroom);
            ValidateDatesAreNotSame(classroom);
            ValidateUpdatedDateIsRecent(classroom);
        }

        private void ValidateAuditFieldsDataOnCreate(Classroom classroom)
        {
            switch (classroom)
            {
                case { } when classroom.UpdatedBy != classroom.CreatedBy:
                    throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.UpdatedBy),
                    parameterValue: classroom.UpdatedBy);

                case { } when classroom.UpdatedDate != classroom.CreatedDate:
                    throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.UpdatedDate),
                    parameterValue: classroom.UpdatedDate);

                case { } when IsDateNotRecent(classroom.CreatedDate):
                    throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.CreatedDate),
                    parameterValue: classroom.CreatedDate);

                case { } when classroom.CreatedDate != classroom.UpdatedDate:
                    throw new InvalidClassroomException(
                        parameterName: nameof(Classroom.UpdatedDate),
                        parameterValue: classroom.UpdatedDate);
            }
        }

        private static void ValidateInvalidAuditFields(Classroom classroom)
        {
            switch (classroom)
            {
                case { } when IsInvalid(classroom.CreatedBy):
                    throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.CreatedBy),
                    parameterValue: classroom.CreatedBy);

                case { } when IsInvalid(classroom.CreatedDate):
                    throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.CreatedDate),
                    parameterValue: classroom.CreatedDate);

                case { } when IsInvalid(classroom.UpdatedBy):
                    throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.UpdatedBy),
                    parameterValue: classroom.UpdatedBy);

                case { } when IsInvalid(classroom.UpdatedDate):
                    throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.UpdatedDate),
                    parameterValue: classroom.UpdatedDate);
            }
        }

        private static void ValidateClassroomFields(Classroom classroom)
        {
            if (IsInvalid(classroom.Name))
            {
                throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.Name),
                    parameterValue: classroom.Name);
            }
        }

        private static void ValidateClassroomIsNull(Classroom classroom)
        {
            if (classroom is null)
            {
                throw new NullClassroomException();
            }
        }

        private static void ValidateClassroomIdIsNull(Guid classroomId)
        {
            if (classroomId == default)
            {
                throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.Id),
                    parameterValue: classroomId);
            }
        }

        private static void ValidateDatesAreNotSame(Classroom classroom)
        {
            if (classroom.CreatedDate == classroom.UpdatedDate)
            {
                throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.UpdatedDate),
                    parameterValue: classroom.UpdatedDate);
            }
        }

        private void ValidateUpdatedDateIsRecent(Classroom classroom)
        {
            if (IsDateNotRecent(classroom.UpdatedDate))
            {
                throw new InvalidClassroomException(
                    parameterName: nameof(classroom.UpdatedDate),
                    parameterValue: classroom.UpdatedDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private static void ValidateStorageClassroom(Classroom storageClassroom, Guid classroomId)
        {
            if (storageClassroom == null)
            {
                throw new NotFoundClassroomException(classroomId);
            }
        }

        private static void ValidateAgainstStorageClassroomOnModify(Classroom inputClassroom, Classroom storageClassroom)
        {
            switch (inputClassroom)
            {
                case { } when inputClassroom.CreatedDate != storageClassroom.CreatedDate:
                    throw new InvalidClassroomException(
                        parameterName: nameof(Classroom.CreatedDate),
                        parameterValue: inputClassroom.CreatedDate);

                case { } when inputClassroom.CreatedBy != storageClassroom.CreatedBy:
                    throw new InvalidClassroomException(
                        parameterName: nameof(Classroom.CreatedBy),
                        parameterValue: inputClassroom.CreatedBy);

                case { } when inputClassroom.UpdatedDate == storageClassroom.UpdatedDate:
                    throw new InvalidClassroomException(
                        parameterName: nameof(Classroom.UpdatedDate),
                        parameterValue: inputClassroom.UpdatedDate);
            }
        }

        private void ValidateStorageClassrooms(IQueryable<Classroom> storageClassrooms)
        {
            if (!storageClassrooms.Any())
            {
                this.loggingBroker.LogWarning("No classrooms found in storage.");
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;
        private static bool IsInvalid(DateTimeOffset input) => input == default;
    }
}
