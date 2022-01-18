// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Text.RegularExpressions;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Registrations
{
    public partial class RegistrationService
    {
        private void ValidateRegistrationOnModify(Registration registration)
        {
            ValidateRegistrationNotNull(registration);
            ValidateRegistrationRequiredFields(registration);
        }

        private void ValidateRegistrationRequiredFields(Registration registration)
        {
            switch (registration)
            {
                case { } when IsInvalid(registration.Id):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(registration.Id),
                        parameterValue: registration.Id);

                case { } when IsInvalid(registration.StudentName):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.StudentName),
                        parameterValue: registration.StudentName);

                case { } when IsInvalidEmailAddress(registration.StudentEmail):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.StudentEmail),
                        parameterValue: registration.StudentEmail);

                case { } when IsNotValidPhoneNumber(registration.StudentPhone):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.StudentPhone),
                        parameterValue: registration.StudentPhone);

                case { } when IsInvalid(registration.SubmitterName):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.SubmitterName),
                        parameterValue: registration.SubmitterName);

                case { } when IsInvalidEmailAddress(registration.SubmitterEmail):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.SubmitterEmail),
                        parameterValue: registration.SubmitterEmail);

                case { } when IsNotValidPhoneNumber(registration.SubmitterPhone):
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

                case { } when IsInvalid(registration.CreatedDate):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.CreatedDate),
                        parameterValue: registration.CreatedDate);

                case { } when IsInvalid(registration.UpdatedDate):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.UpdatedDate),
                        parameterValue: registration.UpdatedDate);

                case { } when IsDateNotRecent(registration.UpdatedDate):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.UpdatedDate),
                        parameterValue: registration.UpdatedDate);
            }
        }

        private static void ValidateRegistrationNotNull(Registration registration)
        {
            if (registration is not null)
            {
                throw new NullRegistrationException();
            }
        }

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

                case { } when IsInvalidEmailAddress(registration.StudentEmail):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.StudentEmail),
                        parameterValue: registration.StudentEmail);

                case { } when IsNotValidPhoneNumber(registration.StudentPhone):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.StudentPhone),
                        parameterValue: registration.StudentPhone);

                case { } when IsInvalid(registration.SubmitterName):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.SubmitterName),
                        parameterValue: registration.SubmitterName);

                case { } when IsInvalidEmailAddress(registration.SubmitterEmail):
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.SubmitterEmail),
                        parameterValue: registration.SubmitterEmail);

                case { } when IsNotValidPhoneNumber(registration.SubmitterPhone):
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

        private static void ValidateAgainstStorageRegistrationOnModify(
            Registration inputRegistration,
            Registration storageRegistration)
        {
            switch (inputRegistration)
            {
                case { } when inputRegistration.CreatedDate != storageRegistration.CreatedDate:
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.CreatedDate),
                        parameterValue: inputRegistration.CreatedDate);

                case { } when inputRegistration.CreatedBy != storageRegistration.CreatedBy:
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.CreatedBy),
                        parameterValue: inputRegistration.CreatedBy);

                case { } when inputRegistration.UpdatedDate == storageRegistration.UpdatedDate:
                    throw new InvalidRegistrationException(
                        parameterName: nameof(Registration.UpdatedDate),
                        parameterValue: inputRegistration.UpdatedDate);
            }
        }

        private static void ValidateRegistrationId(Guid registrationId)
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

        private static bool IsInvalid(Guid input) => input == default;

        private static void ValidateStorageRegistration
            (Registration storageRegistration,
            Guid registrationId)
        {
            if (storageRegistration == null)
            {
                throw new NotFoundRegistrationException(registrationId);
            }
        }

        private void ValidateStorageRegistrations(IQueryable<Registration> storageRegistrations)
        {
            if (!storageRegistrations.Any())
            {
                this.loggingBroker.LogWarning("No Registrations found in storage.");
            }
        }

        private static bool IsInvalidEmailAddress(string emailAddress)
        {
            bool isInvalid = IsInvalid(emailAddress);

            return isInvalid || IsValidEmailFormat(emailAddress) == false;
        }

        private static bool IsValidEmailFormat(string emailAddress)
        {
            return Regex.IsMatch(
                input: emailAddress,
                pattern: @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
                options: RegexOptions.IgnoreCase);
        }

        private static bool IsNotValidPhoneNumber(string phoneNumber) =>
            string.IsNullOrWhiteSpace(phoneNumber) || IsValidPhoneNumberFormat(phoneNumber) == false;

        private static bool IsValidPhoneNumberFormat(string phoneNumber) =>
            phoneNumber.All(character => Char.IsDigit(character) || character == '-');

        private static bool IsNotSame(Guid firstId, Guid secondId) => firstId != secondId;

        private static bool IsNotSame(DateTimeOffset firstDate, DateTimeOffset secondDate) =>
            firstDate != secondDate;

        private static bool IsInvalid(DateTimeOffset date) => date == default;
        private static bool IsInvalid(String input) => String.IsNullOrWhiteSpace(input);
    }
}
