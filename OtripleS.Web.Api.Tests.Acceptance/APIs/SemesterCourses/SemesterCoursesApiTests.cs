// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.SemesterCourses
{
    [Collection(nameof(ApiTestCollection))]
    public partial class SemesterCoursesApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public SemesterCoursesApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private IEnumerable<SemesterCourse> GetRandomSemesterCourses() =>
            CreateRandomSemesterCourseFiller().Create(GetRandomNumber());

        private SemesterCourse CreateRandomSemesterCourse() =>
            CreateRandomSemesterCourseFiller().Create();

        private Filler<SemesterCourse> CreateRandomSemesterCourseFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<SemesterCourse>();
            var teacherId = this.otripleSApiBroker.GetAllTeachersAsync().Result.FirstOrDefault().Id;
            var classroomId = this.otripleSApiBroker.GetAllClassroomsAsync().Result.FirstOrDefault().Id;
            var courseId = this.otripleSApiBroker.GetAllCoursesAsync().Result.FirstOrDefault().Id;

            filler.Setup()
                .OnProperty(semesterCourse => semesterCourse.CreatedBy).Use(posterId)
                .OnProperty(semesterCourse => semesterCourse.UpdatedBy).Use(posterId)
                .OnProperty(semesterCourse => semesterCourse.CreatedDate).Use(now)
                .OnProperty(semesterCourse => semesterCourse.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(semesterCourse => semesterCourse.TeacherId).Use(teacherId)
                .OnProperty(semesterCourse => semesterCourse.ClassroomId).Use(classroomId)
                .OnProperty(semesterCourse => semesterCourse.CourseId).Use(courseId)
                .OnProperty(semesterCourse => semesterCourse.Teacher).IgnoreIt()
                .OnProperty(semesterCourse => semesterCourse.Classroom).IgnoreIt()
                .OnProperty(semesterCourse => semesterCourse.Course).IgnoreIt();

            return filler;
        }

        private SemesterCourse UpdateSemesterCourseRandom(SemesterCourse semesterCourse)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var teacherId = this.otripleSApiBroker.GetAllTeachersAsync().Result.FirstOrDefault().Id;
            var classroomId = this.otripleSApiBroker.GetAllClassroomsAsync().Result.FirstOrDefault().Id;
            var courseId = this.otripleSApiBroker.GetAllCoursesAsync().Result.FirstOrDefault().Id;

            var filler = new Filler<SemesterCourse>();

            filler.Setup()
                .OnProperty(semesterCourse => semesterCourse.Id).Use(semesterCourse.Id)
                .OnProperty(semesterCourse => semesterCourse.CreatedBy).Use(semesterCourse.CreatedBy)
                .OnProperty(semesterCourse => semesterCourse.UpdatedBy).Use(semesterCourse.UpdatedBy)
                .OnProperty(semesterCourse => semesterCourse.CreatedDate).Use(semesterCourse.CreatedDate)
                .OnProperty(semesterCourse => semesterCourse.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(semesterCourse => semesterCourse.TeacherId).Use(teacherId)
                .OnProperty(semesterCourse => semesterCourse.ClassroomId).Use(classroomId)
                .OnProperty(semesterCourse => semesterCourse.CourseId).Use(courseId)
                .OnProperty(semesterCourse => semesterCourse.Teacher).IgnoreIt()
                .OnProperty(semesterCourse => semesterCourse.Classroom).IgnoreIt()
                .OnProperty(semesterCourse => semesterCourse.Course).IgnoreIt();

            return filler.Create();
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}
