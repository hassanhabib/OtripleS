using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Assignments.Exceptions
{
    public class AssignmentValidationException : Exception
    {
        public AssignmentValidationException(Exception innerException)
            : base("Invalid input, contact support.", innerException) { }
    }
}
