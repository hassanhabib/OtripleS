// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Guardian;
using OtripleS.Web.Api.Models.Guardian.Exceptions;

namespace OtripleS.Web.Api.Services.Guardians
{
    public partial class GuardianService
    {
        private delegate ValueTask<Guardian> ReturningGuardianFunction();

        private async ValueTask<Guardian> TryCatch(ReturningGuardianFunction returningGuardianFunction)
        {
            try
            {
                return await returningGuardianFunction();
            }
            catch (InvalidGuardianException invalidGuardianInputException)
            {
                throw CreateAndLogValidationException(invalidGuardianInputException);
            }
        }

        private GuardianValidationException CreateAndLogValidationException(Exception exception)
        {
            var GuardianValidationException = new GuardianValidationException(exception);
            this.loggingBroker.LogError(GuardianValidationException);

            return GuardianValidationException;
        }

        private void ValidateGuardianId(Guid guardianId)
        {
            if (guardianId == default)
            {
                throw new InvalidGuardianException(
                    parameterName: nameof(Guardian.Id),
                    parameterValue: guardianId);
            }
        }
    }
}
