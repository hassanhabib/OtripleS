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
           : base($"Couldn't find Student Semester Course with StudentId: {studentId} " +
                  $"and SemesterCourseId: {semesterCourseId}.")
        { }
    }
}
