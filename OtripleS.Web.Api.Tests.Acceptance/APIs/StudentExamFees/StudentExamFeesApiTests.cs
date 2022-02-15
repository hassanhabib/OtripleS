// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Classrooms;
using OtripleS.Web.Api.Tests.Acceptance.Models.Courses;
using OtripleS.Web.Api.Tests.Acceptance.Models.ExamFees;
using OtripleS.Web.Api.Tests.Acceptance.Models.Exams;
using OtripleS.Web.Api.Tests.Acceptance.Models.Fees;
using OtripleS.Web.Api.Tests.Acceptance.Models.SemesterCourses;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentExamFees;
using OtripleS.Web.Api.Tests.Acceptance.Models.Students;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Users;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentExamFees
{
    [Collection(nameof(ApiTestCollection))]
    public partial class StudentExamFeesApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public StudentExamFeesApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private async ValueTask<StudentExamFee> DeleteStudentExamFeeAsync(StudentExamFee studentExamFee)
        {
            StudentExamFee deletedStudentExamFee =
                await this.otripleSApiBroker.DeleteStudentExamFeeByIdsAsync(
                    studentExamFee.StudentId,
                    studentExamFee.ExamFeeId);

            await this.otripleSApiBroker.DeleteStudentByIdAsync(deletedStudentExamFee.StudentId);

            ExamFee deletedExamFee =
                await this.otripleSApiBroker.DeleteExamFeeByIdAsync(deletedStudentExamFee.ExamFeeId);

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

            return deletedStudentExamFee;
        }

        private async ValueTask<StudentExamFee> PostRandomStudentExamFeeAsync()
        {
            StudentExamFee studentExamFee = await CreateRandomStudentExamFeeAsync();

            return await this.otripleSApiBroker.PostStudentExamFeeAsync(studentExamFee);
        }

        private async ValueTask<Fee> PostRandomFeeAsync(User user)
        {
            Fee fee = CreateRandomFeeFiller(user);

            return await this.otripleSApiBroker.PostFeeAsync(fee);
        }

        private async ValueTask<ExamFee> PostRandomExamFeeAsync(User user)
        {
            ExamFee examFee = await CreateRandomExamFeeAsync(user);

            return await this.otripleSApiBroker.PostExamFeeAsync(examFee);
        }

        private async ValueTask<Student> PostRandomStudentAsync(User user)
        {
            Student randomStudent = CreateRandomStudent(user);
            await this.otripleSApiBroker.PostStudentAsync(randomStudent);

            return randomStudent;
        }

        private async ValueTask<Exam> PostExamAsync(User user)
        {
            Exam randomExam = await CreateRandomExamAsync(user);
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

        private async ValueTask<StudentExamFee> CreateRandomStudentExamFeeAsync() =>
            await CreateRandomStudentExamFeeFillerAsync();

        private static Student CreateRandomStudent(User user) =>
           CreateRandomStudentFiller(user).Create();

        private async ValueTask<ExamFee> CreateRandomExamFeeAsync(User user) =>
            await CreateRandomExamFeeFiller(user);

        private async Task<StudentExamFee> CreateRandomStudentExamFeeFillerAsync()
        {
            var filler = new Filler<StudentExamFee>();
            DateTimeOffset now = DateTimeOffset.UtcNow;
            User user = await PostRandomUserAsync();
            Student student = await PostRandomStudentAsync(user);
            ExamFee examFee = await PostRandomExamFeeAsync(user);

            filler.Setup()
                .OnProperty(studentExamFee => studentExamFee.StudentId).Use(student.Id)
                .OnProperty(studentExamFee => studentExamFee.ExamFeeId).Use(examFee.Id)
                .OnProperty(studentExamFee => studentExamFee.CreatedBy).Use(user.Id)
                .OnProperty(studentExamFee => studentExamFee.UpdatedBy).Use(user.Id)
                .OnProperty(studentExamFee => studentExamFee.CreatedDate).Use(now)
                .OnProperty(studentExamFee => studentExamFee.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static ValueTask<StudentExamFee> UpdateStudentExamFeeRandom(StudentExamFee studentExamFee)
        {
            studentExamFee.UpdatedDate = DateTimeOffset.UtcNow;

            return new ValueTask<StudentExamFee>(studentExamFee);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomString() => new MnemonicString().GetValue();
        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static Fee CreateRandomFeeFiller(User user)
        {
            var now = DateTimeOffset.UtcNow;
            var filler = new Filler<Fee>();

            filler.Setup()
                .OnProperty(fee => fee.CreatedBy).Use(user.Id)
                .OnProperty(fee => fee.UpdatedBy).Use(user.Id)
                .OnProperty(fee => fee.CreatedDate).Use(now)
                .OnProperty(fee => fee.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static Filler<Student> CreateRandomStudentFiller(User user)
        {
            var now = DateTimeOffset.UtcNow;
            var userId = Guid.NewGuid();
            var filler = new Filler<Student>();

            filler.Setup()
                .OnProperty(student => student.CreatedBy).Use(user.Id)
                .OnProperty(student => student.UpdatedBy).Use(user.Id)
                .OnProperty(student => student.CreatedDate).Use(now)
                .OnProperty(student => student.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private async ValueTask<ExamFee> CreateRandomExamFeeFiller(User user)
        {
            var filler = new Filler<ExamFee>();
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Fee fee = await PostRandomFeeAsync(user);
            Exam exam = await PostExamAsync(user);

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

        private static Filler<User> CreateRandomUserFiller()
        {
            var now = DateTimeOffset.UtcNow;
            DateTimeOffset? nullableDateTime = null;
            var filler = new Filler<User>();

            filler.Setup()
                .OnProperty(user => user.CreatedDate).Use(now)
                .OnProperty(user => user.UpdatedDate).Use(now)
                .OnProperty(user => user.LockoutEnd).Use(nullableDateTime)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private async ValueTask<Exam> CreateRandomExamAsync(User user)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            SemesterCourse semesterCourse = await PostRandomSemesterCourseAsync();
            var filler = new Filler<Exam>();

            filler.Setup()
                .OnProperty(exam => exam.CreatedBy).Use(user.Id)
                .OnProperty(exam => exam.UpdatedBy).Use(user.Id)
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
                .OnProperty(teacher => teacher.Status).Use(TeacherStatus.Active)
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
            var now = DateTimeOffset.UtcNow;
            var posterId = Guid.NewGuid();
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

        private async ValueTask<Course> PostCourseAsync()
        {
            Course randomCourse = CreateRandomCourseFiller().Create();

            return await this.otripleSApiBroker.PostCourseAsync(randomCourse);
        }

        private static Filler<Course> CreateRandomCourseFiller()
        {
            var now = DateTimeOffset.UtcNow;
            var posterId = Guid.NewGuid();
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