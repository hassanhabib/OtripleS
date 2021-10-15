// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.SemesterCourses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<SemesterCourse> InsertSemesterCourseAsync(SemesterCourse semesterCourse);
        IQueryable<SemesterCourse> SelectAllSemesterCourses();
        ValueTask<SemesterCourse> SelectSemesterCourseByIdAsync(Guid semesterCourseId);
        ValueTask<SemesterCourse> UpdateSemesterCourseAsync(SemesterCourse semesterCourse);
        ValueTask<SemesterCourse> DeleteSemesterCourseAsync(SemesterCourse semesterCourse);
    }
}
