using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Guardian.Exceptions
{
    public class GuardianDependencyException:Exception
    {
        public GuardianDependencyException(Exception innerException)
        : base("Service dependency error occurred, contact support.", innerException) { }
    }
}
