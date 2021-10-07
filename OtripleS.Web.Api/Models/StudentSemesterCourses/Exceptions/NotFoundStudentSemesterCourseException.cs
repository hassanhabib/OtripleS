// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions
{
    public class NotFoundStudentSemesterCourseException : Exception
    {
        public NotFoundStudentSemesterCourseException(Guid studentId, Guid semesterCourseId)
           : base(message: $"Couldn't find student semester course with student id: {studentId} " +
                  $"and semester course id: {semesterCourseId}.")
        { }
    }
}
