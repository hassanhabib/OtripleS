// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;
using System;
using System.Linq;

namespace OtripleS.Web.Api.Services.Foundations.UserContacts
{
    public partial class UserContactService
    {

        private static void ValidateUserContactOnAdd(UserContact userContact)
        {
            ValidateUserContactIsNull(userContact);
            ValidateUserContactRequiredFields(userContact);
        }

        private static void ValidateUserContactIsNull(UserContact userContact)
        {
            if (userContact is null)
            {
                throw new NullUserContactException();
            }
        }

        private void ValidateStorageUserContacts(IQueryable<UserContact> storageUserContacts)
        {
            if (!storageUserContacts.Any())
            {
                this.loggingBroker.LogWarning("No UserContacts found in storage.");
            }
        }

        private static void ValidateUserContactRequiredFields(UserContact userContact)
        {
            switch (userContact)
            {
                case { } when IsInvalid(userContact.UserId):
                    throw new InvalidUserContactInputException(
                        parameterName: nameof(UserContact.UserId),
                        parameterValue: userContact.UserId);

                case { } when IsInvalid(userContact.ContactId):
                    throw new InvalidUserContactInputException(
                        parameterName: nameof(UserContact.ContactId),
                        parameterValue: userContact.ContactId);
            }
        }

        private static void ValidateUserContactIdIsNull(Guid userId, Guid contactId)
        {
            if (userId == default)
            {
                throw new InvalidUserContactInputException(
                    parameterName: nameof(UserContact.UserId),
                    parameterValue: userId);
            }

            if (contactId == default)
            {
                throw new InvalidUserContactInputException(
                    parameterName: nameof(UserContact.ContactId),
                    parameterValue: contactId);
            }
        }

        private static void ValidateStorageUserContact(UserContact storageUserContact, Guid userId, Guid contactId)
        {
            if (storageUserContact == null)
            {
                throw new NotFoundUserContactException(userId, contactId);
            }
        }

        private static bool IsInvalid(Guid input) => input == default;
    }
}
