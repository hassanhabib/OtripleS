using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class FailedTeacherServiceException : Xeption
    {
        public FailedTeacherServiceException(Exception innerException)
            : base(message: "Failed teacher service error occured. ",innerException)
        {

        }
    }
}
