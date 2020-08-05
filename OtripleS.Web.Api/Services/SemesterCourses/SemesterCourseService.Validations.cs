// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.SemesterCourses
{
    public partial class SemesterCourseService
    {
        private void ValidateStorageSemesterCourses(IQueryable<SemesterCourse> storageSemesterCourses)
        {
            if (storageSemesterCourses.Count() == 0)
            {
                this.loggingBroker.LogWarning("No courses for semester found in storage.");
            }
        }
    }
}
