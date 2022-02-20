// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class TeacherServiceException : Xeption
    {
        public TeacherServiceException(Xeption innerException)
            : base(message: "Service error occurred, contact support.", innerException) { }  
    }
}
