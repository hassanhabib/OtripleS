// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Force.DeepCloner;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private IEnumerable<SemesterCourse> CreateRandomSemesterCourses() =>
            Enumerable.Range(start: 0, count: GetRandomNumber())
                .Select(item => CreateRandomSemesterCourse());

        private SemesterCourse CreateExpectedSemesterCourse(SemesterCourse semesterCourse)
        {
            SemesterCourse expectedSemesterCourse = semesterCourse.DeepClone();
            expectedSemesterCourse.Teacher = null;
            expectedSemesterCourse.Classroom = null;
            expectedSemesterCourse.Course = null;

            return expectedSemesterCourse;
        }

        private SemesterCourse CreateRandomSemesterCourse() =>
            CreateRandomSemesterCourseFiller().Create();

        private Filler<SemesterCourse> CreateRandomSemesterCourseFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Teacher randomTeacher = CreateRandomTeacherFiller().Create();
            Classroom randomClassroom = CreateRandomClassroomFiller().Create();
            Course randomCourse = CreateRandomCourseFiller().Create();
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<SemesterCourse>();

            filler.Setup()
                .OnProperty(semesterCourse => semesterCourse.CreatedBy).Use(posterId)
                .OnProperty(semesterCourse => semesterCourse.UpdatedBy).Use(posterId)
                .OnProperty(semesterCourse => semesterCourse.CreatedDate).Use(now)
                .OnProperty(semesterCourse => semesterCourse.UpdatedDate).Use(now)
                .OnProperty(semesterCourse => semesterCourse.Teacher).Use(randomTeacher)
                .OnProperty(semesterCourse => semesterCourse.Classroom).Use(randomClassroom)
                .OnProperty(semesterCourse => semesterCourse.Course).Use(randomCourse)
                .OnProperty(semesterCourse => semesterCourse.TeacherId).Use(randomTeacher.Id)
                .OnProperty(semesterCourse => semesterCourse.ClassroomId).Use(randomClassroom.Id)
                .OnProperty(semesterCourse => semesterCourse.CourseId).Use(randomCourse.Id)
                .OnProperty(semesterCourse => semesterCourse.StudentSemesterCourses).IgnoreIt()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private SemesterCourse UpdateSemesterCourseRandom(SemesterCourse semesterCourse)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<SemesterCourse>();

            filler.Setup()
                .OnProperty(semesterCourse => semesterCourse.Id).Use(semesterCourse.Id)
                .OnProperty(semesterCourse => semesterCourse.CreatedBy).Use(semesterCourse.CreatedBy)
                .OnProperty(semesterCourse => semesterCourse.UpdatedBy).Use(semesterCourse.UpdatedBy)
                .OnProperty(semesterCourse => semesterCourse.CreatedDate).Use(semesterCourse.CreatedDate)
                .OnProperty(semesterCourse => semesterCourse.UpdatedDate).Use(now)
                .OnProperty(semesterCourse => semesterCourse.TeacherId).Use(semesterCourse.TeacherId)
                .OnProperty(semesterCourse => semesterCourse.ClassroomId).Use(semesterCourse.ClassroomId)
                .OnProperty(semesterCourse => semesterCourse.CourseId).Use(semesterCourse.CourseId)
                .OnProperty(semesterCourse => semesterCourse.Teacher).Use(semesterCourse.Teacher)
                .OnProperty(semesterCourse => semesterCourse.Classroom).Use(semesterCourse.Classroom)
                .OnProperty(semesterCourse => semesterCourse.Course).Use(semesterCourse.Course)
                .OnProperty(semesterCourse => semesterCourse.StudentSemesterCourses).IgnoreIt()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private Filler<Classroom> CreateRandomClassroomFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Classroom>();

            filler.Setup()
                .OnProperty(classroom => classroom.CreatedBy).Use(posterId)
                .OnProperty(classroom => classroom.UpdatedBy).Use(posterId)
                .OnProperty(classroom => classroom.CreatedDate).Use(now)
                .OnProperty(classroom => classroom.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(classroom => classroom.SemesterCourses).IgnoreIt();

            return filler;
        }

        private Filler<Teacher> CreateRandomTeacherFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Teacher>();

            filler.Setup()
                .OnProperty(teacher => teacher.CreatedBy).Use(posterId)
                .OnProperty(teacher => teacher.UpdatedBy).Use(posterId)
                .OnProperty(teacher => teacher.CreatedDate).Use(now)
                .OnProperty(teacher => teacher.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(teacher => teacher.SemesterCourses).IgnoreIt()
                .OnProperty(teacher => teacher.TeacherContacts).IgnoreIt();

            return filler;
        }

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
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(classroom => classroom.SemesterCourses).IgnoreIt();

            return filler;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}
