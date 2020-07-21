// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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

    }
}
