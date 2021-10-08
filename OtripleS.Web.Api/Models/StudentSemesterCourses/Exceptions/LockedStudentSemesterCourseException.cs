// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class LockedStudentSemesterCourseException : Exception
    {
        public LockedStudentSemesterCourseException(Exception innerException)
            : base(message: "Locked student semester course record exception, please try again later.", innerException) { }
    }
}
