// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Attachments;
using OtripleS.Web.Api.Tests.Acceptance.Models.Classrooms;
using OtripleS.Web.Api.Tests.Acceptance.Models.Courses;
using OtripleS.Web.Api.Tests.Acceptance.Models.Exams;
using OtripleS.Web.Api.Tests.Acceptance.Models.ExamsAttachments;
using OtripleS.Web.Api.Tests.Acceptance.Models.SemesterCourses;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.ExamsAttachments
{
    [Collection(nameof(ApiTestCollection))]
    public partial class ExamAttachmentsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public ExamAttachmentsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private async Task<ExamAttachment> CreateRandomExamAttachment()
        {
            Exam persistedExam = await PostExamAsync();
            Attachment persistedAttachment = await PostAttachmentAsync();

            ExamAttachment randomExamAttachment = CreateRandomExamAttachmentFiller(
                persistedExam.Id,
                persistedAttachment.Id).Create();

            return randomExamAttachment;
        }

        private async Task<ExamAttachment> PostExamAttachmentAsync()
        {
            ExamAttachment randomExamAttachment = await CreateRandomExamAttachment();
            await this.otripleSApiBroker.PostExamAttachmentAsync(randomExamAttachment);

            return randomExamAttachment;
        }

        private async ValueTask<ExamAttachment> DeleteExamAttachmentAsync(ExamAttachment examAttachment)
        {
            ExamAttachment deletedExamAttachment =
                await this.otripleSApiBroker.DeleteExamAttachmentByIdsAsync(
                    examAttachment.ExamId,
                    examAttachment.AttachmentId);

            await this.otripleSApiBroker.DeleteAttachmentByIdAsync(examAttachment.AttachmentId);

            Exam deletedExam =
                await this.otripleSApiBroker.DeleteExamByIdAsync(examAttachment.ExamId);

            SemesterCourse deletedSemesterCourse =
                await this.otripleSApiBroker.DeleteSemesterCourseByIdAsync(deletedExam.SemesterCourseId);

            await this.otripleSApiBroker.DeleteTeacherByIdAsync(deletedSemesterCourse.TeacherId);
            await this.otripleSApiBroker.DeleteClassroomByIdAsync(deletedSemesterCourse.ClassroomId);
            await this.otripleSApiBroker.DeleteCourseByIdAsync(deletedSemesterCourse.CourseId);

            return deletedExamAttachment;
        }

        private static Attachment CreateRandomAttachment() => CreateRandomAttachmentFiller().Create();

        private async ValueTask<Exam> PostExamAsync()
        {
            Exam randomExam = await CreateRandomExamAsync();
            Exam inputExam = randomExam;

            return await this.otripleSApiBroker.PostExamAsync(inputExam);
        }

        private async ValueTask<Attachment> PostAttachmentAsync()
        {
            Attachment randomAttachment = CreateRandomAttachment();
            Attachment inputAttachment = randomAttachment;

            return await this.otripleSApiBroker.PostAttachmentAsync(inputAttachment);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 1, max: 5).GetValue();

        private static Filler<ExamAttachment> CreateRandomExamAttachmentFiller(Guid examId, Guid attachmentId)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<ExamAttachment>();

            filler.Setup()
                .OnProperty(examAttachment => examAttachment.ExamId).Use(examId)
                .OnProperty(examAttachment => examAttachment.AttachmentId).Use(attachmentId)
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
                .OnProperty(course => course.Status).Use(CourseStatus.Available)
                .OnProperty(course => course.CreatedBy).Use(posterId)
                .OnProperty(course => course.UpdatedBy).Use(posterId)
                .OnProperty(course => course.CreatedDate).Use(now)
                .OnProperty(course => course.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static Filler<Attachment> CreateRandomAttachmentFiller()
        {
            var now = DateTimeOffset.UtcNow;
            var posterId = Guid.NewGuid();
            var filler = new Filler<Attachment>();

            filler.Setup()
                .OnProperty(attachment => attachment.CreatedBy).Use(posterId)
                .OnProperty(attachment => attachment.UpdatedBy).Use(posterId)
                .OnProperty(attachment => attachment.CreatedDate).Use(now)
                .OnProperty(attachment => attachment.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}