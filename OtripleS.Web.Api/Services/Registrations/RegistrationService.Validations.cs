// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;

namespace OtripleS.Web.Api.Services.Registrations
{
    public partial class RegistrationService
    {
        private void ValidateRegistrationOnAdd(Registration registration)
        {
            switch (registration)
            {
                case null:
                    throw new NullRegistrationException();

                case { } when IsInvalid(registration.Id):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(registration.Id),
                        parameterValue: registration.Id);

                case { } when IsInvalid(registration.CreatedBy):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.CreatedBy),
                        parameterValue: registration.CreatedBy);

            }
        }

        private void ValidateRegistrationIsNotNull(Registration registration)
        {
            if (registration == default)
            {
                throw new NullRegistrationException();
            }
        }

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

        private void ValidateStorageRegistrations(IQueryable<Registration> storageRegistrations)
        {
            if (storageRegistrations.Count() == 0)
            {
                this.loggingBroker.LogWarning("No Registrations found in storage.");
            }
        }
    }
}
