using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class FailedStudentStorageException : Xeption
    {
        public FailedStudentStorageException(Exception innerException)
            : base(message: "Failed post storage error occurred, contact suppport.", innerException)
        { }
    }
}
