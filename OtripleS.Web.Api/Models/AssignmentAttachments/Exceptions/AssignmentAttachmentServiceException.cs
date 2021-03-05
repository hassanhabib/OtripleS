using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions
{
    public class AssignmentAttachmentServiceException : Exception
    {
        public AssignmentAttachmentServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
