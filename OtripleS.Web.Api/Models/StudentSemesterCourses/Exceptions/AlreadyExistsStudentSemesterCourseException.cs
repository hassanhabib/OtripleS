//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class AlreadyExistsStudentSemesterCourseException : Exception
    {
        public AlreadyExistsStudentSemesterCourseException(Exception innerException)
            : base("StudentSemesterCourse with the same id already exists.", innerException) { }
    }
}
