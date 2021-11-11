// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Attendances;
using OtripleS.Web.Api.Tests.Acceptance.Models.Classrooms;
using OtripleS.Web.Api.Tests.Acceptance.Models.Courses;
using OtripleS.Web.Api.Tests.Acceptance.Models.SemesterCourses;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Attendances
{
    [Collection(nameof(ApiTestCollection))]
    public partial class AttendancesApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public AttendancesApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static Attendance UpdateAttendanceRandom(Attendance inputAttendance)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            inputAttendance.UpdatedDate = now;

            return inputAttendance;
        }

        private async ValueTask<Attendance> PostRandomAttendanceAsync()
        {
            Attendance randomAttendance = await CreateRandomAttendanceAsync();
            await this.otripleSApiBroker.PostAttendanceAsync(randomAttendance);

            return randomAttendance;
        }

        private async ValueTask<Attendance> CreateRandomAttendanceAsync()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Teacher randomTeacher = await PostTeacherAsync();
            Classroom randomClassroom = await PostClassRoomAsync();
            Course randomCourse = await PostCourseAsync();

            SemesterCourse semesterCourse = await PostRandomSemesterCourseAsync(
                teacher: randomTeacher,
                classroom: randomClassroom,
                course: randomCourse);

            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Attendance>();

            filler.Setup()
                .OnProperty(attendance => attendance.CreatedBy).Use(posterId)
                .OnProperty(attendance => attendance.UpdatedBy).Use(posterId)
                .OnProperty(attendance => attendance.CreatedDate).Use(now)
                .OnProperty(attendance => attendance.UpdatedDate).Use(now)
                .OnProperty(Attendance => Attendance.AttendanceDate).Use(now)
                .OnProperty(Attendance => Attendance.StudentSemesterCourseId).Use(semesterCourse.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private async ValueTask<Attendance> DeleteAttendanceByIdAsync(Attendance attendance)
        {
            Attendance deletedAttendance =
                await this.otripleSApiBroker.DeleteAttendanceByIdAsync(attendance.Id);

            SemesterCourse deletedSemesterCourse =
                await this.otripleSApiBroker.DeleteSemesterCourseByIdAsync(
                    attendance.StudentSemesterCourseId);

            await this.otripleSApiBroker.DeleteCourseByIdAsync(deletedSemesterCourse.CourseId);
            await this.otripleSApiBroker.DeleteClassroomByIdAsync(deletedSemesterCourse.ClassroomId);
            await this.otripleSApiBroker.DeleteTeacherByIdAsync(deletedSemesterCourse.TeacherId);

            return deletedAttendance;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private async ValueTask<SemesterCourse> PostRandomSemesterCourseAsync(
            Teacher teacher,
            Classroom classroom,
            Course course)
        {
            SemesterCourse randomSemesterCourse =
                CreateRandomSemesterCourse(
                    teacher: teacher,
                    classroom: classroom,
                    course: course);

            await this.otripleSApiBroker.PostSemesterCourseAsync(randomSemesterCourse);

            return randomSemesterCourse;
        }

        private static SemesterCourse CreateRandomSemesterCourse(
            Teacher teacher,
            Classroom classroom,
            Course course)
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

            return semesterCourse;
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

        private async ValueTask<Teacher> PostTeacherAsync()
        {
            Teacher randomTeacher = CreateRandomTeacherFiller().Create();

            return await this.otripleSApiBroker.PostTeacherAsync(randomTeacher);
        }

        private static Filler<Classroom> CreateRandomClassroomFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<Classroom>();

            filler.Setup()
                .OnProperty(classroom => classroom.Status).Use(ClassroomStatus.Available)
                .OnProperty(classroom => classroom.CreatedBy).Use(posterId)
                .OnProperty(classroom => classroom.UpdatedBy).Use(posterId)
                .OnProperty(classroom => classroom.CreatedDate).Use(now)
                .OnProperty(classroom => classroom.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static Filler<Teacher> CreateRandomTeacherFiller()
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

        private static Filler<Course> CreateRandomCourseFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<Course>();

            filler.Setup()
                .OnProperty(course => course.Status).Use(CourseStatus.Available)
                .OnProperty(course => course.CreatedBy).Use(posterId)
                .OnProperty(course => course.UpdatedBy).Use(posterId)
                .OnProperty(course => course.CreatedDate).Use(now)
                .OnProperty(course => course.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}