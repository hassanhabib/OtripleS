using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class StudentServiceException : Exception
    {
        public StudentServiceException(Exception innerException) 
            : base("Service error occurred, contact support.", innerException) { }
    }
}
