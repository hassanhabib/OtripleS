// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Courses;

namespace OtripleS.Web.Api.Services.Courses
{
    public interface ICourseService
    {
        ValueTask<Course> ModifyCourseAsync(Course course);
        ValueTask<Course> DeleteCourseAsync(Guid CourseId);
    }
}
