using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Guardian.Exceptions
{
    public class NotFoundGuardianException:Exception
    {
        public NotFoundGuardianException(Guid guardianId)
        : base($"Couldn't find guardian with Id: {guardianId}.") { }
    }
}
