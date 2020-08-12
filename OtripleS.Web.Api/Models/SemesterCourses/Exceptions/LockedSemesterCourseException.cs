// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.SemesterCourses.Exceptions
{
    public class LockedSemesterCourseException : Exception
    {
        public LockedSemesterCourseException(Exception innerException)
            : base("Locked semesterCourse record exception, please try again later.", innerException) { }
    }
}
