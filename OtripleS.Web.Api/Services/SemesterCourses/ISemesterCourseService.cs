// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.SemesterCourses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.SemesterCourses
{
    public interface ISemesterCourseService
    {
        ValueTask<SemesterCourse> CreateSemesterCourseAsync(SemesterCourse semesterCourse);
        IQueryable<SemesterCourse> RetrieveAllSemesterCourses();
        ValueTask<SemesterCourse> RetrieveSemesterCourseByIdAsync(Guid semesterCourseId);
        ValueTask<SemesterCourse> ModifySemesterCourseAsync(SemesterCourse semesterCourse);
        ValueTask<SemesterCourse> DeleteSemesterCourseAsync(Guid semesterCourseId);
    }
}
