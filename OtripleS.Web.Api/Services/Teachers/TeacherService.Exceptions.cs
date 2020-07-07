// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;

namespace OtripleS.Web.Api.Services.Teachers
{
    public partial class TeacherService
    {
        private delegate ValueTask<Teacher> ReturningTeacherFunction();

        private async ValueTask<Teacher> TryCatch(ReturningTeacherFunction returningTeacherFunction)
        {
            try
            {
                return await returningTeacherFunction();
            }
            catch (InvalidTeacherInputException invalidTeacherInputException)
            {
                throw CreateAndLogValidationException(invalidTeacherInputException);
            }
        }

        private TeacherValidationException CreateAndLogValidationException(Exception exception)
        {
            var teacherValidationException = new TeacherValidationException(exception);
            this.loggingBroker.LogError(teacherValidationException);

            return teacherValidationException;
        }
    }
}
