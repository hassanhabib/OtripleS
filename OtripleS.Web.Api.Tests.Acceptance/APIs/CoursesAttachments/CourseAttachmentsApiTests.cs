// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Attachments;
using OtripleS.Web.Api.Tests.Acceptance.Models.Courses;
using OtripleS.Web.Api.Tests.Acceptance.Models.CoursesAttachments;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.CoursesAttachments
{
    [Collection(nameof(ApiTestCollection))]
    public partial class CourseAttachmentsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public CourseAttachmentsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private async Task<CourseAttachment> CreateRandomCourseAttachment()
        {
            Course persistedCourse = await PostCourseAsync();
            Attachment persistedAttachment = await PostAttachmentAsync();

            CourseAttachment randomCourseAttachment = CreateRandomCourseAttachmentFiller(
                persistedCourse.Id,
                persistedAttachment.Id).Create();

            return randomCourseAttachment;
        }

        private async Task<CourseAttachment> PostCourseAttachmentAsync()
        {
            CourseAttachment randomCourseAttachment = await CreateRandomCourseAttachment();
            await this.otripleSApiBroker.PostCourseAttachmentAsync(randomCourseAttachment);

            return randomCourseAttachment;
        }

        private async ValueTask<CourseAttachment> DeleteCourseAttachmentAsync(CourseAttachment courseAttachment)
        {
            CourseAttachment deletedCourseAttachment =
                await this.otripleSApiBroker.DeleteCourseAttachmentByIdsAsync(
                    courseAttachment.CourseId,
                    courseAttachment.AttachmentId);

            await this.otripleSApiBroker.DeleteAttachmentByIdAsync(courseAttachment.AttachmentId);
            await this.otripleSApiBroker.DeleteCourseByIdAsync(courseAttachment.CourseId);

            return deletedCourseAttachment;
        }

        private static Course CreateRandomCourse() => CreateRandomCourseFiller().Create();
        private static Attachment CreateRandomAttachment() => CreateRandomAttachmentFiller().Create();

        private async ValueTask<Course> PostCourseAsync()
        {
            Course randomCourse = CreateRandomCourse();
            Course inputCourse = randomCourse;

            return await this.otripleSApiBroker.PostCourseAsync(inputCourse);
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

        private static Filler<CourseAttachment> CreateRandomCourseAttachmentFiller(Guid courseId, Guid attachmentId)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<CourseAttachment>();

            filler.Setup()
                .OnProperty(courseAttachment => courseAttachment.CourseId).Use(courseId)
                .OnProperty(courseAttachment => courseAttachment.AttachmentId).Use(attachmentId)
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

        private static Filler<Attachment> CreateRandomAttachmentFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
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