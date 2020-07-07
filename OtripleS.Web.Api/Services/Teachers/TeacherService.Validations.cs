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
    }
}
