// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.UserContacts
{
    public partial class UserContactService
    {

        private static void ValidateUserContactOnAdd(UserContact userContact)
        {
            ValidateUserContactIsNull(userContact);
            ValidateUserContactIds(userContact.UserId, userContact.ContactId);
        }

        private static void ValidateUserContactIds(Guid userId, Guid contactId)
        {
            Validate(
               (Rule: IsInvalid(contactId), Parameter: nameof(UserContact.ContactId)),
               (Rule: IsInvalid(userId), Parameter: nameof(UserContact.UserId)));
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

        private static void ValidateStorageUserContact(UserContact storageUserContact, Guid userId, Guid contactId)
        {
            if (storageUserContact == null)
            {
                throw new NotFoundUserContactException(userId, contactId);
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidStudentException = new InvalidUserContactInputException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidStudentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidStudentException.ThrowIfContainsErrors();
        }
    }
}
