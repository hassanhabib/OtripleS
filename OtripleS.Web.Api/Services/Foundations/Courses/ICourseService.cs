﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Courses;

namespace OtripleS.Web.Api.Services.Foundations.Courses
{
    public interface ICourseService
    {
        ValueTask<Course> CreateCourseAsync(Course course);
        IQueryable<Course> RetrieveAllCourses();
        ValueTask<Course> RetrieveCourseByIdAsync(Guid courseId);
        ValueTask<Course> ModifyCourseAsync(Course course);
        ValueTask<Course> RemoveCourseAsync(Guid courseId);
    }
}
