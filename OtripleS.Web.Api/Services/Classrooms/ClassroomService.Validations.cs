// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;

namespace OtripleS.Web.Api.Services.Classrooms
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
                    throw new InvalidClassroomInputException(
                    parameterName: nameof(Classroom.UpdatedBy),
                    parameterValue: classroom.UpdatedBy);

                case { } when classroom.UpdatedDate != classroom.CreatedDate:
                    throw new InvalidClassroomInputException(
                    parameterName: nameof(Classroom.UpdatedDate),
                    parameterValue: classroom.UpdatedDate);

                case { } when IsDateNotRecent(classroom.CreatedDate):
                    throw new InvalidClassroomInputException(
                    parameterName: nameof(Classroom.CreatedDate),
                    parameterValue: classroom.CreatedDate);

                case { } when classroom.CreatedDate != classroom.UpdatedDate:
                    throw new InvalidClassroomInputException(
                        parameterName: nameof(Classroom.UpdatedDate),
                        parameterValue: classroom.UpdatedDate);
            }
        }

        private void ValidateInvalidAuditFields(Classroom classroom)
        {
            switch (classroom)
            {
                case { } when IsInvalid(classroom.CreatedBy):
                    throw new InvalidClassroomInputException(
                    parameterName: nameof(Classroom.CreatedBy),
                    parameterValue: classroom.CreatedBy);

                case { } when IsInvalid(classroom.CreatedDate):
                    throw new InvalidClassroomInputException(
                    parameterName: nameof(Classroom.CreatedDate),
                    parameterValue: classroom.CreatedDate);

                case { } when IsInvalid(classroom.UpdatedBy):
                    throw new InvalidClassroomInputException(
                    parameterName: nameof(Classroom.UpdatedBy),
                    parameterValue: classroom.UpdatedBy);

                case { } when IsInvalid(classroom.UpdatedDate):
                    throw new InvalidClassroomInputException(
                    parameterName: nameof(Classroom.UpdatedDate),
                    parameterValue: classroom.UpdatedDate);
            }
        }

        private void ValidateClassroomFields(Classroom classroom)
        {
            if (IsInvalid(classroom.Name))
            {
                throw new InvalidClassroomInputException(
                    parameterName: nameof(Classroom.Name),
                    parameterValue: classroom.Name);
            }
        }

        private void ValidateClassroomIsNull(Classroom classroom)
        {
            if (classroom is null)
            {
                throw new NullClassroomException();
            }
        }

        private void ValidateClassroomIdIsNull(Guid classroomId)
        {
            if (classroomId == default)
            {
                throw new InvalidClassroomInputException(
                    parameterName: nameof(Classroom.Id),
                    parameterValue: classroomId);
            }
        }

        private void ValidateDatesAreNotSame(Classroom classroom)
        {
            if (classroom.CreatedDate == classroom.UpdatedDate)
            {
                throw new InvalidClassroomInputException(
                    parameterName: nameof(Classroom.UpdatedDate),
                    parameterValue: classroom.UpdatedDate);
            }
        }

        private void ValidateUpdatedDateIsRecent(Classroom classroom)
        {
            if (IsDateNotRecent(classroom.UpdatedDate))
            {
                throw new InvalidClassroomInputException(
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

        private void ValidateStorageClassroom(Classroom storageClassroom, Guid classroomId)
        {
            if (storageClassroom == null)
            {
                throw new NotFoundClassroomException(classroomId);
            }
        }

        private void ValidateAgainstStorageClassroomOnModify(Classroom inputClassroom, Classroom storageClassroom)
        {
            switch (inputClassroom)
            {
                case { } when inputClassroom.CreatedDate != storageClassroom.CreatedDate:
                    throw new InvalidClassroomInputException(
                        parameterName: nameof(Classroom.CreatedDate),
                        parameterValue: inputClassroom.CreatedDate);

                case { } when inputClassroom.CreatedBy != storageClassroom.CreatedBy:
                    throw new InvalidClassroomInputException(
                        parameterName: nameof(Classroom.CreatedBy),
                        parameterValue: inputClassroom.CreatedBy);

                case { } when inputClassroom.UpdatedDate == storageClassroom.UpdatedDate:
                    throw new InvalidClassroomInputException(
                        parameterName: nameof(Classroom.UpdatedDate),
                        parameterValue: inputClassroom.UpdatedDate);
            }
        }

        private void ValidateStorageClassrooms(IQueryable<Classroom> storageClassrooms)
        {
            if (storageClassrooms.Count() == 0)
            {
                this.loggingBroker.LogWarning("No classrooms found in storage.");
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(Guid input) => input == default;
        private static bool IsInvalid(DateTimeOffset input) => input == default;
    }
}
