// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.SemesterCourses.Exceptions
{
    public class FailedSemesterCourseServiceException : Xeption
    {
        public FailedSemesterCourseServiceException(Exception innerException)
            : base(message: "Failed semester course service error occurred, contact support.", innerException)
        {
        }
    }
}
