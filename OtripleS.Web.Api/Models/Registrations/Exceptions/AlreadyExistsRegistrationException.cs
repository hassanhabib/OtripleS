using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Registrations.Exceptions
{
    public class AlreadyExistsRegistrationException : Exception
    {
        public AlreadyExistsRegistrationException(Exception innerException)
            : base("Registration with the same id already exists.", innerException) { }
    }
}
