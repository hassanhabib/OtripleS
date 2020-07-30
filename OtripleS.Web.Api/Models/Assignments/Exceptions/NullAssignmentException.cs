using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Assignments.Exceptions
{
    public class NullAssignmentException : Exception
    {
        public NullAssignmentException() : base("The assignment is null")
        { }
    }
}
