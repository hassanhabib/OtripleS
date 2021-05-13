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

                case { } when IsInvalid(registration.StudentName):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.StudentName),
                        parameterValue: registration.StudentName);

                case { } when IsInvalid(registration.StudentEmail):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.StudentEmail),
                        parameterValue: registration.StudentEmail);

                case { } when IsInvalid(registration.StudentPhone):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.StudentPhone),
                        parameterValue: registration.StudentPhone);

                case { } when IsInvalid(registration.SubmitterName):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.SubmitterName),
                        parameterValue: registration.SubmitterName);                
                
                case { } when IsInvalid(registration.SubmitterEmail):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.SubmitterEmail),
                        parameterValue: registration.SubmitterEmail);                
                
                case { } when IsInvalid(registration.SubmitterPhone):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.SubmitterPhone),
                        parameterValue: registration.SubmitterPhone);

                case { } when IsInvalid(registration.CreatedBy):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.CreatedBy),
                        parameterValue: registration.CreatedBy);

                case { } when IsInvalid(registration.UpdatedBy):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.UpdatedBy),
                        parameterValue: registration.UpdatedBy);

                case { } when IsNotSame(registration.CreatedDate, registration.UpdatedDate):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.UpdatedDate),
                        parameterValue: registration.UpdatedDate);

                case { } when IsNotSame(registration.CreatedBy, registration.UpdatedBy):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.UpdatedBy),
                        parameterValue: registration.UpdatedBy);

                case { } when IsDateNotRecent(registration.CreatedDate):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.CreatedDate),
                        parameterValue: registration.CreatedDate);
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

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
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

        private static bool IsNotSame(Guid firstId, Guid secondId) => firstId != secondId;
        private static bool IsNotSame(DateTimeOffset firstDate, DateTimeOffset secondDate) =>
            firstDate != secondDate;
        private static bool IsInvalid(DateTimeOffset date) => date == default;
        private static bool IsInvalid(String input) => String.IsNullOrWhiteSpace(input);
    }
}
