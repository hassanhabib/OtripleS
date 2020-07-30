using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Assignments.Exceptions
{
    public class NotFoundAssignmentException : Exception
    {
        public NotFoundAssignmentException(Guid assignmentId)
            : base($"Couldn't find course with Id: {assignmentId}.") { }
    }
}
