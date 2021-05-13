// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;

namespace OtripleS.Web.Api.Services.Registrations
{
    public partial class RegistrationService
    {
        private void ValidateRegistrationId(Guid registrationId)
        {
            if (IsInvalid(registrationId))
            {
                throw new InvalidRegistrationException(
                    parameterName: nameof(Registration.Id),
                    parameterValue: registrationId);
            }
        }

        private bool IsInvalid(Guid input) => input == default;

        private void ValidateStorageRegistration(Registration storageRegistration, Guid registrationId)
        {
            if (storageRegistration == null)
            {
                throw new NotFoundRegistrationException(registrationId);
            }
        }
    }
}
