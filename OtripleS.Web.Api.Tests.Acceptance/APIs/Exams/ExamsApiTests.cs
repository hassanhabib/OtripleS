// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Classrooms;
using OtripleS.Web.Api.Tests.Acceptance.Models.Courses;
using OtripleS.Web.Api.Tests.Acceptance.Models.SemesterCourses;
using OtripleS.Web.Api.Tests.Acceptance.Models.Students;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Exams;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Exams
{
    [Collection(nameof(ApiTestCollection))]
    public partial class ExamsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public ExamsApiTests(OtripleSApiBroker otripleSApiBroker)
        {
            this.otripleSApiBroker = otripleSApiBroker;
        }

        private async ValueTask<Exam> DeleteExamAsync(Exam exam)
        {
            Exam deletedExam =
                await this.otripleSApiBroker.DeleteExamByIdAsync(exam.Id);

            SemesterCourse deletedSemesterCourse =
                await this.otripleSApiBroker.DeleteSemesterCourseByIdAsync(deletedExam.SemesterCourseId);

            await this.otripleSApiBroker.DeleteCourseByIdAsync(deletedSemesterCourse.CourseId);
            await this.otripleSApiBroker.DeleteClassroomByIdAsync(deletedSemesterCourse.ClassroomId);
            await this.otripleSApiBroker.DeleteTeacherByIdAsync(deletedSemesterCourse.TeacherId);

            return deletedExam;
        }

        private async ValueTask<Exam> PostRandomExamAsync()
        {
            Exam exam = await CreateRandomExamAsync();

            return await this.otripleSApiBroker.PostExamAsync(exam);
        }

        private async ValueTask<Exam> CreateRandomExamAsync()
        {
            Teacher randomTeacher = await PostRandomTeacherAsync();
            Course randomCourse = await PostRandomCourseAsync();
            Classroom randomClassroom = await PostRandomClassroom();
            
            SemesterCourse semesterCourse = 
                await PostRandomSemesterCourseAsync(randomTeacher, randomCourse, randomClassroom);

            return CreateRandomExamFiller(semesterCourse).Create();
        }

        private async ValueTask<Teacher> PostRandomTeacherAsync()
        {
            Teacher teacher = CreateRandomTeacherFiller().Create();

            return await this.otripleSApiBroker.PostTeacherAsync(teacher);
        }

        private async ValueTask<SemesterCourse> PostRandomSemesterCourseAsync(
            Teacher teacher,
            Course course,
            Classroom classroom)
        {
            SemesterCourse semesterCourse =
                CreateRandomSemesterCourseFiller(teacher, course, classroom).Create();

            return await this.otripleSApiBroker.PostSemesterCourseAsync(semesterCourse);
        }

        private async ValueTask<Course> PostRandomCourseAsync()
        {
            Course course = CreateRandomCourseFiller().Create();

            return await this.otripleSApiBroker.PostCourseAsync(course);
        }

        private async ValueTask<Classroom> PostRandomClassroom()
        {
            Classroom classroom = CreateRandomClassroomFiller().Create();

            return await this.otripleSApiBroker.PostClassroomAsync(classroom);
        }

        private async ValueTask<Exam> UpdateExamRandom(Exam exam)
        {
            exam.Label = GetRandomString();
            exam.UpdatedDate = DateTimeOffset.UtcNow;

            return exam;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private string GetRandomString() => new MnemonicString().GetValue();
        private int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private Filler<Teacher> CreateRandomTeacherFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid userId = Guid.NewGuid();

            var filler = new Filler<Teacher>();

            filler.Setup()
                .OnProperty(teacher => teacher.CreatedBy).Use(userId)
                .OnProperty(teacher => teacher.UpdatedBy).Use(userId)
                .OnProperty(teacher => teacher.CreatedDate).Use(now)
                .OnProperty(teacher => teacher.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private Filler<Exam> CreateRandomExamFiller(SemesterCourse semesterCourse)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid userId = Guid.NewGuid();

            var filler = new Filler<Exam>();

            filler.Setup()
                .OnProperty(exam => exam.CreatedBy).Use(userId)
                .OnProperty(exam => exam.UpdatedBy).Use(userId)
                .OnProperty(exam => exam.CreatedDate).Use(now)
                .OnProperty(exam => exam.UpdatedDate).Use(now)
                .OnProperty(exam => exam.SemesterCourseId).Use(semesterCourse.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private Filler<SemesterCourse> CreateRandomSemesterCourseFiller(
            Teacher teacher,
            Course course,
            Classroom classroom)
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
                .OnProperty(semesterCourse => semesterCourse.CourseId).Use(course.Id)
                .OnProperty(semesterCourse => semesterCourse.ClassroomId).Use(classroom.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private Filler<Course> CreateRandomCourseFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid userId = Guid.NewGuid();

            var filler = new Filler<Course>();

            filler.Setup()
                .OnProperty(course => course.CreatedBy).Use(userId)
                .OnProperty(course => course.UpdatedBy).Use(userId)
                .OnProperty(course => course.CreatedDate).Use(now)
                .OnProperty(course => course.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private Filler<Classroom> CreateRandomClassroomFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid userId = Guid.NewGuid();

            var filler = new Filler<Classroom>();

            filler.Setup()
                .OnProperty(classroom => classroom.CreatedBy).Use(userId)
                .OnProperty(classroom => classroom.UpdatedBy).Use(userId)
                .OnProperty(classroom => classroom.CreatedDate).Use(now)
                .OnProperty(classroom => classroom.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}
