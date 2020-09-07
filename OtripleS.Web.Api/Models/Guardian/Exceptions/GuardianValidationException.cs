using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Guardian.Exceptions
{
    public class GuardianValidationException: Exception
    {
        public GuardianValidationException(Exception innerException)
        : base("Invalid input, contact support.", innerException) { }
    }
    
    
}
