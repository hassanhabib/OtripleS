using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Guardian.Exceptions
{
    public class AlreadyExistsGuardianException:Exception
    {
        public AlreadyExistsGuardianException(Exception innerException)
        : base("Guardian with the same id already exists.", innerException) { }
    }
}
