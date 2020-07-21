// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;

namespace OtripleS.Web.Api.Services.Classrooms
{
    public partial class ClassroomService
    {
        private void ValidateClassroom(Classroom classroom)
        {
            ValidateClassroomIsNull(classroom);
            ValidateClassroomIdIsNull(classroom);
            ValidateClassroomFields(classroom);
            ValidateAuditFieldsOnCreate(classroom);
        }

        private void ValidateAuditFieldsOnCreate(Classroom classroom)
        {
            switch (classroom)
            {
                case { } when classroom.CreatedBy == default:
                    throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.CreatedBy),
                    parameterValue: classroom.CreatedBy);

                case { } when classroom.CreatedDate == default:
                    throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.CreatedDate),
                    parameterValue: classroom.CreatedDate);

                case { } when classroom.UpdatedBy == default ||
                                classroom.UpdatedBy != classroom.CreatedBy:
                    throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.UpdatedBy),
                    parameterValue: classroom.UpdatedBy);

                case { } when classroom.UpdatedDate == default ||
                                classroom.UpdatedDate != classroom.CreatedDate:
                    throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.UpdatedDate),
                    parameterValue: classroom.UpdatedDate);
            }
        }

        private void ValidateClassroomFields(Classroom classroom)
        {
            if (IsEmpty(classroom.Name))
            {
                throw new InvalidClassroomException(
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

        private void ValidateClassroomIdIsNull(Classroom classroom)
        {
            if (classroom.Id == default)
            {
                throw new InvalidClassroomException(
                    parameterName: nameof(Classroom.Id),
                    parameterValue: classroom.Id);
            }
        }

        private static bool IsEmpty(string input) => String.IsNullOrWhiteSpace(input);
    }
}
