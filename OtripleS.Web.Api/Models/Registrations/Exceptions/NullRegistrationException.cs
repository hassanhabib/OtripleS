using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Registrations.Exceptions
{
    public class NullRegistrationException : Exception
    {
        public NullRegistrationException() : base("The registration is null.") { }
    }
}
