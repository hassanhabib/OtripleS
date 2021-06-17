using System;

namespace OtripleS.Web.Api.Models.Foundations.Registrations.Exceptions
{
    public class AlreadyExistsRegistrationException : Exception
    {
        public AlreadyExistsRegistrationException(Exception innerException)
            : base("Registration with the same id already exists.", innerException) { }
    }
}
