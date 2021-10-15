//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Models.StudentSemesterCourses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.StudentSemesterCourses
{
    public interface IStudentSemesterCourseService
    {
        ValueTask<StudentSemesterCourse> RetrieveStudentSemesterCourseByIdAsync(Guid studentId, Guid semesterCourse);

        ValueTask<StudentSemesterCourse> CreateStudentSemesterCourseAsync(
            StudentSemesterCourse inputStudentSemesterCourse);

        IQueryable<StudentSemesterCourse> RetrieveAllStudentSemesterCourses();
        ValueTask<StudentSemesterCourse> ModifyStudentSemesterCourseAsync(StudentSemesterCourse studentSemesterCourse);
        ValueTask<StudentSemesterCourse> RemoveStudentSemesterCourseByIdsAsync(Guid semesterCourseId, Guid studentId);
    }
}
