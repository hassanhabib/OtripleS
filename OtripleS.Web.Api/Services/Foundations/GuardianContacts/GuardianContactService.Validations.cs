//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.GuardianContacts;
using OtripleS.Web.Api.Models.GuardianContacts.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.GuardianContacts
{
    public partial class GuardianContactService
    {
        private static void ValidateGuardianContactOnCreate(GuardianContact guardianContact)
        {
            ValidateGuardianContactIsNull(guardianContact);
            ValidateGuardianContactRequiredFields(guardianContact);
        }

        private void ValidateStorageGuardianContacts(IQueryable<GuardianContact> storageGuardianContacts)
        {
            if (!storageGuardianContacts.Any())
            {
                this.loggingBroker.LogWarning("No GuardianContacts found in storage.");
            }
        }

        private static void ValidateGuardianContactIsNull(GuardianContact guardianContact)
        {
            if (guardianContact is null)
            {
                throw new NullGuardianContactException();
            }
        }

        private static void ValidateGuardianContactIdIsNull(Guid guardianId, Guid contactId)
        {
            if (guardianId == default)
            {
                throw new InvalidGuardianContactInputException(
                    parameterName: nameof(GuardianContact.GuardianId),
                    parameterValue: guardianId);
            }
            if (contactId == default)
            {
                throw new InvalidGuardianContactInputException(
                    parameterName: nameof(GuardianContact.ContactId),
                    parameterValue: contactId);
            }
        }

        private static void ValidateGuardianContactRequiredFields(GuardianContact guardianContact)
        {
            switch (guardianContact)
            {
                case { } when IsInvalid(guardianContact.GuardianId):
                    throw new InvalidGuardianContactInputException(
                        parameterName: nameof(GuardianContact.GuardianId),
                        parameterValue: guardianContact.GuardianId);

                case { } when IsInvalid(guardianContact.ContactId):
                    throw new InvalidGuardianContactInputException(
                        parameterName: nameof(GuardianContact.ContactId),
                        parameterValue: guardianContact.ContactId);
            }
        }

        private static void ValidateStorageGuardianContact(
            GuardianContact storageGuardianContact,
            Guid guardianId, Guid contactId)
        {
            if (storageGuardianContact == null)
            {
                throw new NotFoundGuardianContactException(guardianId, contactId);
            }
        }

        private static bool IsInvalid(Guid input) => input == default;
    }
}