//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.StudentSemesterCourses
{
    public interface IStudentSemesterCourseService
    {
        public ValueTask<StudentSemesterCourse> CreateStudentSemesterCourseAsync(StudentSemesterCourse inputStudentSemesterCourse);
        IQueryable<StudentSemesterCourse> RetrieveAllStudentSemesterCourses();
        ValueTask<StudentSemesterCourse> ModifyStudentSemesterCourseAsync(StudentSemesterCourse studentSemesterCourse);
        ValueTask<StudentSemesterCourse> DeleteStudentSemesterCourseAsync(Guid semesterCourseId, Guid studentId);
    }
}
