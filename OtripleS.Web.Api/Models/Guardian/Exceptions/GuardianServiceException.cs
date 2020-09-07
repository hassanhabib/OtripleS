using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Guardian.Exceptions
{
    public class GuardianServiceException:Exception
    {
        public GuardianServiceException(Exception innerException)
        : base("Service error occurred, contact support.", innerException) { }
    }
}
