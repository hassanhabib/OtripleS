// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Courses;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Courses
{
    [Collection(nameof(ApiTestCollection))]
    public partial class CoursesApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public CoursesApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private IEnumerable<Course> GetRandomCourses() =>
            CreateRandomCourseFiller().Create(GetRandomNumber());

        private Course CreateRandomCourse() =>
            CreateRandomCourseFiller().Create();

        private Filler<Course> CreateRandomCourseFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<Course>();

            filler.Setup()
                .OnProperty(course => course.CreatedBy).Use(posterId)
                .OnProperty(course => course.UpdatedBy).Use(posterId)
                .OnProperty(course => course.CreatedDate).Use(now)
                .OnProperty(course => course.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private Course UpdateCourseRandom(Course course)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Course>();

            filler.Setup()
                .OnProperty(course => course.Id).Use(course.Id)
                .OnProperty(course => course.CreatedBy).Use(course.CreatedBy)
                .OnProperty(course => course.UpdatedBy).Use(course.UpdatedBy)
                .OnProperty(course => course.CreatedDate).Use(course.CreatedDate)
                .OnProperty(course => course.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}
