using System;
namespace OtripleS.Web.Api.Models.Registrations.Exceptions
{
    public class NotFoundRegistrationException : Exception
    {
        public NotFoundRegistrationException(Guid registrationId)
            : base($"Couldn't find registration with Id: {registrationId}.") { }
    }
}
