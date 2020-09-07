using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Guardian.Exceptions
{
    public class NullGuardianException:Exception
    {
        public NullGuardianException() : base("The guardian is null.")
        {

        }
       
    }
}
