using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Contacts.Exceptions
{
    public class FailedContactStorageException : Xeption
    {
        public FailedContactStorageException(Exception innerException)
            :base("Failed contact storage error occurred, contact support.", innerException)
        { }
    }
}
