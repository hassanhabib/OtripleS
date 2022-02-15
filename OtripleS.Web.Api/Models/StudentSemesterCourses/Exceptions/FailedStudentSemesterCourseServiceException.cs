// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class FailedStudentSemesterCourseServiceException : Xeption
    {
        public FailedStudentSemesterCourseServiceException(Exception innerException)
            : base(message: "Failed Student Semester Course Service error ocuured.", innerException)
        {
        }
    }
}
