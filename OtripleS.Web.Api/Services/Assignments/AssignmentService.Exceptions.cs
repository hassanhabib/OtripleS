// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;

namespace OtripleS.Web.Api.Services.Assignments
{
    public partial class AssignmentService
    {
        private delegate ValueTask<Assignment> ReturningAssignmentFunction();

        private async ValueTask<Assignment> TryCatch(ReturningAssignmentFunction returningAssignmentFunction)
        {
            try
            {
                return await returningAssignmentFunction();
            }
            catch (NullAssignmentException nullClassroomException)
            {
                throw CreateAndLogValidationException(nullClassroomException);
            }
            catch (InvalidAssignmentException invalidAssignmentException)
            {
                throw CreateAndLogValidationException(invalidAssignmentException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsClassroomException =
                    new AlreadyExistsAssignmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsClassroomException);
            }
        }

        private AssignmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var assignmentValidationException = new AssignmentValidationException(exception);
            this.loggingBroker.LogError(assignmentValidationException);

            return assignmentValidationException;
        }
    }
}
