using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Assignments.Exceptions
{
    public class FailedAssignmentServiceException :Xeption
    {
        public FailedAssignmentServiceException(Exception innerException)
             : base("Failed Assignment service error occured, contact support", innerException)
        {}
    }
}
