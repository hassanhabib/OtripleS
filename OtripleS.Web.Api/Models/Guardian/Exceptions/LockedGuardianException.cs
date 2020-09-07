using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Guardian.Exceptions
{
    public class LockedGuardianException:Exception
    {
        public LockedGuardianException(Exception innerException)
        : base("Locked Guardian record exception, please try again later.", innerException) { }
    }
}
