// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;

namespace OtripleS.Web.Api.Services.Registrations
{
    public partial class RegistrationService
    {
        private delegate ValueTask<Registration> ReturningRegistrationFunction();

        private async ValueTask<Registration> TryCatch(ReturningRegistrationFunction returningRegistrationFunction)
        {
            try
            {
                return await returningRegistrationFunction();
            }
            catch (InvalidRegistrationException invalidRegistrationInputException)
            {
                throw CreateAndLogValidationException(invalidRegistrationInputException);
            }
        }

        private RegistrationValidationException CreateAndLogValidationException(Exception exception)
        {
            var RegistrationValidationException = new RegistrationValidationException(exception);
            this.loggingBroker.LogError(RegistrationValidationException);

            return RegistrationValidationException;
        }
    }
}
