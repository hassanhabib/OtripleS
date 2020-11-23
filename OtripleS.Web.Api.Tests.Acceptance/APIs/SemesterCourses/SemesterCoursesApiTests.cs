// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Classrooms;
using OtripleS.Web.Api.Tests.Acceptance.Models.Courses;
using OtripleS.Web.Api.Tests.Acceptance.Models.SemesterCourses;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;
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

        private SemesterCourse CreateExpectedSemesterCourse(SemesterCourse semesterCourse)
        {
            SemesterCourse expectedSemesterCourse = semesterCourse.DeepClone();

            return expectedSemesterCourse;
        }

        private async ValueTask<SemesterCourse> CreateRandomSemesterCourseAsync()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Teacher randomTeacher = await PostTeacherAsync();
            Classroom randomClassroom = await PostClassRoomAsync();
            Course randomCourse = await PostCourseAsync();
            Guid userId = Guid.NewGuid();
            var filler = new Filler<SemesterCourse>();

            filler.Setup()
                .OnProperty(semesterCourse => semesterCourse.CreatedBy).Use(userId)
                .OnProperty(semesterCourse => semesterCourse.UpdatedBy).Use(userId)
                .OnProperty(semesterCourse => semesterCourse.CreatedDate).Use(now)
                .OnProperty(semesterCourse => semesterCourse.UpdatedDate).Use(now)
                .OnProperty(semesterCourse => semesterCourse.TeacherId).Use(randomTeacher.Id)
                .OnProperty(semesterCourse => semesterCourse.ClassroomId).Use(randomClassroom.Id)
                .OnProperty(semesterCourse => semesterCourse.CourseId).Use(randomCourse.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            SemesterCourse semesterCourse = filler.Create();

            return semesterCourse;
        }

        private async ValueTask<SemesterCourse> CreateRandomSemesterCourseAsync(
            Teacher teacher, Classroom classroom, Course course)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid userId = Guid.NewGuid();
            var filler = new Filler<SemesterCourse>();

            filler.Setup()
                .OnProperty(semesterCourse => semesterCourse.CreatedBy).Use(userId)
                .OnProperty(semesterCourse => semesterCourse.UpdatedBy).Use(userId)
                .OnProperty(semesterCourse => semesterCourse.CreatedDate).Use(now)
                .OnProperty(semesterCourse => semesterCourse.UpdatedDate).Use(now)
                .OnProperty(semesterCourse => semesterCourse.TeacherId).Use(teacher.Id)
                .OnProperty(semesterCourse => semesterCourse.ClassroomId).Use(classroom.Id)
                .OnProperty(semesterCourse => semesterCourse.CourseId).Use(course.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            SemesterCourse semesterCourse = filler.Create();

            return await this.otripleSApiBroker.PostSemesterCourseAsync(semesterCourse); ;
        }

        private async ValueTask<Course> PostCourseAsync()
        {
            Course randomCourse = CreateRandomCourseFiller().Create();

            return await this.otripleSApiBroker.PostCourseAsync(randomCourse);
        }

        private async ValueTask<Classroom> PostClassRoomAsync()
        {
            Classroom randomClassroom = CreateRandomClassroomFiller().Create();

            return await this.otripleSApiBroker.PostClassroomAsync(randomClassroom);
        }

        private async Task DeleteSemesterCourseAsync(SemesterCourse semesterCourse)
        {
            await this.otripleSApiBroker.DeleteSemesterCourseByIdAsync(semesterCourse.Id);
            await this.otripleSApiBroker.DeleteCourseByIdAsync(semesterCourse.CourseId);
            await this.otripleSApiBroker.DeleteClassroomByIdAsync(semesterCourse.ClassroomId);
            await this.otripleSApiBroker.DeleteTeacherByIdAsync(semesterCourse.TeacherId);
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
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private async ValueTask<Teacher> PostTeacherAsync()
        {
            Teacher randomTeacher = CreateRandomTeacherFiller().Create();

            return await this.otripleSApiBroker.PostTeacherAsync(randomTeacher);
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
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

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
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

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
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}
