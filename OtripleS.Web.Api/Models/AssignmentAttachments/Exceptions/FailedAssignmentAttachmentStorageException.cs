using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xeptions;

namespace OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions
{
    public class FailedAssignmentAttachmentStorageException : Xeption
    {
        public FailedAssignmentAttachmentStorageException(Exception innerException)
            : base(message: "Failed assignment attachment storage error occurred, contact suppport.", innerException)
        {}
    }
}
