using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.TeacherContacts.Exceptions
{
    public class FailedTeacherContactServiceException : Xeption
    {
        public FailedTeacherContactServiceException(Exception innerException)
            : base(message: "Failed teacher contact service error occured.", innerException)
        { 
        }
    }
}
