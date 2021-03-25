using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Fees.Exceptions
{
    public class FeeServiceException : Exception
    {
        public FeeServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
