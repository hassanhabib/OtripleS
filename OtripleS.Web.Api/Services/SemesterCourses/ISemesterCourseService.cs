// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.SemesterCourses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.SemesterCourses
{
    public interface ISemesterCourseService
    {
        IQueryable<SemesterCourse> RetrieveAllSemesterCourses();
    }
}
