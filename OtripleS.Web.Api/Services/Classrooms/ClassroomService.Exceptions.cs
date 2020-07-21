// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;

namespace OtripleS.Web.Api.Services.Classrooms
{
    public partial class ClassroomService
    {
        private delegate ValueTask<Classroom> ReturningClassroomFunction();

        private async ValueTask<Classroom> TryCatch(ReturningClassroomFunction returningClassroomFunction)
        {
            try
            {
                return await returningClassroomFunction();
            }
            catch (NullClassroomException nullClassroomException)
            {
                throw CreateAndLogValidationException(nullClassroomException);
            }
        }

        private ClassroomValidationException CreateAndLogValidationException(Exception exception)
        {
            var classroomValidationException = new ClassroomValidationException(exception);
            this.loggingBroker.LogError(classroomValidationException);

            return classroomValidationException;
        }
    }
}
