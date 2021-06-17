using System;

namespace OtripleS.Web.Api.Models.Foundations.Registrations.Exceptions
{
    public class NullRegistrationException : Exception
    {
        public NullRegistrationException() : base("The registration is null.") { }
    }
}
