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

        private static bool IsInvalid(Guid input) => input == Guid.Empty;
    }
}
