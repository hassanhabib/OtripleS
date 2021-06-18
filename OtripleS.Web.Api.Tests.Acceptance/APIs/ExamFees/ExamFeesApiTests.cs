// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.Classrooms;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.Courses;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.ExamFees;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.Exams;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.Fees;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.SemesterCourses;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.Teachers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.Users;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.ExamFees
{
    [Collection(nameof(ApiTestCollection))]
    public partial class ExamFeesApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public ExamFeesApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private async ValueTask<ExamFee> DeleteExamFeeAsync(ExamFee examFee)
        {
            ExamFee deletedExamFee =
                await this.otripleSApiBroker.DeleteExamFeeByIdAsync(examFee.Id);

            await this.otripleSApiBroker.DeleteFeeByIdAsync(deletedExamFee.FeeId);
            await this.otripleSApiBroker.DeleteUserByIdAsync(deletedExamFee.CreatedBy);

            Exam deletedExam =
                await this.otripleSApiBroker.DeleteExamByIdAsync(deletedExamFee.ExamId);

            SemesterCourse deletedSemesterCourse =
                await this.otripleSApiBroker.DeleteSemesterCourseByIdAsync(
                    deletedExam.SemesterCourseId);

            await this.otripleSApiBroker.DeleteTeacherByIdAsync(deletedSemesterCourse.TeacherId);
            await this.otripleSApiBroker.DeleteClassroomByIdAsync(deletedSemesterCourse.ClassroomId);
            await this.otripleSApiBroker.DeleteCourseByIdAsync(deletedSemesterCourse.CourseId);

            return deletedExamFee;
        }

        private async ValueTask<ExamFee> PostRandomExamFeeAsync()
        {
            ExamFee examFee = await CreateRandomExamFeeAsync();

            return await this.otripleSApiBroker.PostExamFeeAsync(examFee);
        }

        private async ValueTask<Fee> PostRandomFeeAsync(User user)
        {
            Fee fee = await CreateRandomFeeFiller(user);

            return await this.otripleSApiBroker.PostFeeAsync(fee);
        }

        private async ValueTask<Exam> PostExamAsync()
        {
            Exam randomExam = await CreateRandomExamAsync();
            Exam inputExam = randomExam;

            return await this.otripleSApiBroker.PostExamAsync(inputExam);
        }

        private async ValueTask<User> PostRandomUserAsync()
        {
            User user = CreateRandomUser();

            return await this.otripleSApiBroker.PostUserAsync(user);
        }

        private static User CreateRandomUser() =>
            CreateRandomUserFiller().Create();

        private async ValueTask<ExamFee> CreateRandomExamFeeAsync() =>
            await CreateRandomExamFeeFiller();

        private async ValueTask<ExamFee> CreateRandomExamFeeFiller()
        {
            var filler = new Filler<ExamFee>();
            DateTimeOffset now = DateTimeOffset.UtcNow;
            User user = await PostRandomUserAsync();
            Fee fee = await PostRandomFeeAsync(user);
            Exam exam = await PostExamAsync();

            filler.Setup()
                .OnProperty(examFee => examFee.FeeId).Use(fee.Id)
                .OnProperty(examFee => examFee.ExamId).Use(exam.Id)
                .OnProperty(examFee => examFee.CreatedBy).Use(user.Id)
                .OnProperty(examFee => examFee.UpdatedBy).Use(user.Id)
                .OnProperty(examFee => examFee.CreatedDate).Use(now)
                .OnProperty(examFee => examFee.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static ValueTask<ExamFee> UpdateExamFeeRandom(ExamFee examFee)
        {
            examFee.UpdatedDate = DateTimeOffset.UtcNow;

            return new ValueTask<ExamFee>(examFee);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomString() => new MnemonicString().GetValue();
        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static ValueTask<Fee> CreateRandomFeeFiller(User user)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Fee>();

            filler.Setup()
                .OnProperty(fee => fee.CreatedBy).Use(user.Id)
                .OnProperty(fee => fee.UpdatedBy).Use(user.Id)
                .OnProperty(fee => fee.CreatedDate).Use(now)
                .OnProperty(fee => fee.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return new ValueTask<Fee>(filler.Create());
        }

        private static Filler<User> CreateRandomUserFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            DateTimeOffset? nullableDateTime = null;
            var filler = new Filler<User>();

            filler.Setup()
                .OnProperty(user => user.CreatedDate).Use(now)
                .OnProperty(user => user.UpdatedDate).Use(now)
                .OnProperty(user => user.LockoutEnd).Use(nullableDateTime)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private async ValueTask<Exam> CreateRandomExamAsync()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            SemesterCourse semesterCourse = await PostRandomSemesterCourseAsync();
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<Exam>();

            filler.Setup()
                .OnProperty(exam => exam.CreatedBy).Use(posterId)
                .OnProperty(exam => exam.UpdatedBy).Use(posterId)
                .OnProperty(exam => exam.CreatedDate).Use(now)
                .OnProperty(exam => exam.UpdatedDate).Use(now)
                .OnProperty(exam => exam.SemesterCourseId).Use(semesterCourse.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private async ValueTask<SemesterCourse> PostRandomSemesterCourseAsync()
        {
            SemesterCourse randomSemesterCourse = await CreateRandomSemesterCourseAsync();
            await this.otripleSApiBroker.PostSemesterCourseAsync(randomSemesterCourse);

            return randomSemesterCourse;
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

        private async ValueTask<Teacher> PostTeacherAsync()
        {
            Teacher randomTeacher = CreateRandomTeacherFiller().Create();

            return await this.otripleSApiBroker.PostTeacherAsync(randomTeacher);
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

        private async ValueTask<Classroom> PostClassRoomAsync()
        {
            Classroom randomClassroom = CreateRandomClassroomFiller().Create();

            return await this.otripleSApiBroker.PostClassroomAsync(randomClassroom);
        }

        private static Filler<Classroom> CreateRandomClassroomFiller()
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

        private async ValueTask<Course> PostCourseAsync()
        {
            Course randomCourse = CreateRandomCourseFiller().Create();

            return await this.otripleSApiBroker.PostCourseAsync(randomCourse);
        }

        private static Filler<Course> CreateRandomCourseFiller()
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
    }
}
