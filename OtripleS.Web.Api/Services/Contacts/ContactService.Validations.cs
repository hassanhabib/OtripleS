// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.Contacts.Exceptions;

namespace OtripleS.Web.Api.Services.Contacts
{
    public partial class ContactService
    {

        private static void ValidateContactOnCreate(Contact contact)
        {
            ValidateContactIsNotNull(contact);
            ValidateContactId(contact);
            ValidateContactAuditFields(contact);
            ValidateContactAuditFieldsOnCreate(contact);
        }

        private static void ValidateContactAuditFields(Contact contact)
        {
            switch(contact)
            {
                case { } when IsInvalid(contact.CreatedBy):
                    throw new InvalidContactException(
                        parameterName: nameof(Contact.CreatedBy), 
                        parameterValue: contact.CreatedBy);

                case { } when IsInvalid(contact.CreatedDate):
                    throw new InvalidContactException(
                        parameterName: nameof(Contact.CreatedDate),
                        parameterValue: contact.CreatedDate);

                case { } when IsInvalid(contact.UpdatedBy):
                    throw new InvalidContactException(
                        parameterName: nameof(Contact.UpdatedBy),
                        parameterValue: contact.UpdatedBy);

                case { } when IsInvalid(contact.UpdatedDate):
                    throw new InvalidContactException(
                        parameterName: nameof(Contact.UpdatedDate),
                        parameterValue: contact.UpdatedDate);
            }
        }
        
        private static void ValidateContactAuditFieldsOnCreate(Contact contact)
        {
            switch (contact)
            {
                case { } when contact.UpdatedBy != contact.CreatedBy:
                    throw new InvalidContactException(
                        parameterName: nameof(Contact.UpdatedBy),
                        parameterValue: contact.UpdatedBy);

                case { } when contact.UpdatedDate != contact.CreatedDate:
                    throw new InvalidContactException(
                        parameterName: nameof(Contact.UpdatedDate),
                        parameterValue: contact.UpdatedDate);
            }
        }
        private static void ValidateContactId(Contact contact)
        {
            if (IsInvalid(contact.Id))
            {
                throw new InvalidContactException(
                    parameterName: nameof(Contact.Id),
                    parameterValue: contact.Id);
            }
        }
        
        private static void ValidateContactIsNotNull(Contact contact)
        {
            if (contact is null)
            {
                throw new NullContactException();
            }
        }

        private static bool IsInvalid(DateTimeOffset inputDate) => inputDate == default;
        private static bool IsInvalid(Guid input) => input == Guid.Empty;
    }
}
