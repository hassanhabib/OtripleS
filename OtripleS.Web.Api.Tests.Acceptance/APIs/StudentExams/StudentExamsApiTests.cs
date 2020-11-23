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
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentExams;
using OtripleS.Web.Api.Tests.Acceptance.Models.Students;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Exams;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentExams
{
    [Collection(nameof(ApiTestCollection))]
    public partial class StudentExamsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public StudentExamsApiTests(OtripleSApiBroker otripleSApiBroker)
        {
            this.otripleSApiBroker = otripleSApiBroker;
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private async ValueTask<StudentExam> PostRandomStudentExamAsync()
        {
            StudentExam studentExam = await CreateRandomStudentExamAsync();

            return await this.otripleSApiBroker.PostStudentExamAsync(studentExam);
        }

        private async ValueTask<StudentExam> CreateRandomStudentExamAsync()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Teacher randomTeacher = await PostRandomTeacherAsync();
            Student randomStudent = await PostRandomStudentAsync();
            Course randomCourse = await PostRandomCourseAsync();
            Classroom randomClassroom = await PostRandomClassroomAsync();
            Exam randomExam = await PostRandomExamAsync(randomTeacher, randomCourse, randomClassroom);

            Guid userId = Guid.NewGuid();
            var filler = new Filler<StudentExam>();

            filler.Setup()
                .OnProperty(studentExam => studentExam.CreatedBy).Use(userId)
                .OnProperty(studentExam => studentExam.UpdatedBy).Use(userId)
                .OnProperty(studentExam => studentExam.CreatedDate).Use(now)
                .OnProperty(studentExam => studentExam.UpdatedDate).Use(now)
                .OnProperty(studentExam => studentExam.StudentId).Use(randomStudent.Id)
                .OnProperty(studentExam => studentExam.ExamId).Use(randomExam.Id)
                .OnProperty(studentExam => studentExam.TeacherId).Use(randomTeacher.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private async ValueTask<StudentExam> DeleteStudentExam(StudentExam studentExam)
        {
            StudentExam deletedStudentExam =
                await this.otripleSApiBroker.DeleteStudentExamByIdAsync(studentExam.Id);

            await this.otripleSApiBroker.DeleteStudentByIdAsync(studentExam.StudentId);

            Exam deletedExam =
                await this.otripleSApiBroker.DeleteExamByIdAsync(studentExam.ExamId);

            SemesterCourse deletedSemesterCourse =
                await this.otripleSApiBroker.DeleteSemesterCourseByIdAsync(deletedExam.SemesterCourseId);

            await this.otripleSApiBroker.DeleteCourseByIdAsync(deletedSemesterCourse.CourseId);
            await this.otripleSApiBroker.DeleteClassroomByIdAsync(deletedSemesterCourse.ClassroomId);
            await this.otripleSApiBroker.DeleteTeacherByIdAsync(studentExam.TeacherId);

            return deletedStudentExam;
        }

        private async ValueTask<Teacher> PostRandomTeacherAsync()
        {
            Teacher teacher = CreateRandomTeacherFiller().Create();

            return await this.otripleSApiBroker.PostTeacherAsync(teacher);
        }

        private async ValueTask<Exam> PostRandomExamAsync(
            Teacher teacher,
            Course course,
            Classroom classroom)
        {
            SemesterCourse semesterCourse = await PostRandomSemesterCourseAsync(teacher, course, classroom);
            Exam exam = CreateRandomExamFiller(semesterCourse).Create();

            return await this.otripleSApiBroker.PostExamAsync(exam);
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

        private async ValueTask<Classroom> PostRandomClassroomAsync()
        {
            Classroom classroom = CreateRandomClassroomFiller().Create();

            return await this.otripleSApiBroker.PostClassroomAsync(classroom);
        }

        private async ValueTask<Student> PostRandomStudentAsync()
        {
            Student student = CreateRandomStudentFiller().Create();

            return await this.otripleSApiBroker.PostStudentAsync(student);
        }

        private async ValueTask<StudentExam> UpdateStudentExamRandomAsync(StudentExam studentExam)
        {
            studentExam.Score = GetRandomNumber();
            studentExam.UpdatedDate = DateTimeOffset.UtcNow;

            return studentExam;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

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

        private Filler<Student> CreateRandomStudentFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid userId = Guid.NewGuid();

            var filler = new Filler<Student>();

            filler.Setup()
                .OnProperty(student => student.CreatedBy).Use(userId)
                .OnProperty(student => student.UpdatedBy).Use(userId)
                .OnProperty(student => student.CreatedDate).Use(now)
                .OnProperty(student => student.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}
