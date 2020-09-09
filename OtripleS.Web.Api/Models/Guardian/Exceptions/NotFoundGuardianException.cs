using System;
namespace OtripleS.Web.Api.Models.Guardian.Exceptions
{
    public class NotFoundGuardianException : Exception
    {
        public NotFoundGuardianException(Guid guardianId)
            : base($"Couldn't find guardian with Id: {guardianId}.") { }
    }
}
