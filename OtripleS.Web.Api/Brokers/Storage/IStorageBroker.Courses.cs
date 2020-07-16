// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Courses;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        ValueTask<Course> InsertCourseAsync(Course course);
        IQueryable<Course> SelectAllCourses();
        ValueTask<Course> SelectCourseByIdAsync(Guid courseId);
        ValueTask<Course> UpdateCourseAsync(Course course);
        ValueTask<Course> DeleteCourseAsync(Course course);
    }
}
