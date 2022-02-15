// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class NullStudentSemesterCourseException : Exception
    {
        public NullStudentSemesterCourseException() : base(message: "The student semester course is null.") { }
    }
}
