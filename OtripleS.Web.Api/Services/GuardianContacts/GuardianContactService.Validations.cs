//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.GuardianContacts;
using OtripleS.Web.Api.Models.GuardianContacts.Exceptions;

namespace OtripleS.Web.Api.Services.GuardianContacts
{
    public partial class GuardianContactService
    {
        private void ValidateGuardianContactOnCreate(GuardianContact guardianContact)
        {
            ValidateGuardianContactIsNull(guardianContact);
            ValidateGuardianContactRequiredFields(guardianContact);
        }

        private void ValidateGuardianContactIsNull(GuardianContact guardianContact)
        {
            if (guardianContact is null)
            {
                throw new NullGuardianContactException();
            }
        }

        private void ValidateGuardianContactIdIsNull(Guid guardianId, Guid contactId)
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

        private void ValidateGuardianContactRequiredFields(GuardianContact guardianContact)
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