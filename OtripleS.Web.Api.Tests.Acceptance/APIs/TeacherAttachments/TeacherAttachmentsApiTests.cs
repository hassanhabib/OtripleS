// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Attachments;
using OtripleS.Web.Api.Tests.Acceptance.Models.TeacherAttachments;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.TeacherAttachments
{
    [Collection(nameof(ApiTestCollection))]
    public partial class TeacherAttachmentsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public TeacherAttachmentsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private async Task<TeacherAttachment> CreateRandomTeacherAttachment()
        {
            Teacher persistedTeacher = await PostTeacherAsync();
            Attachment persistedAttachment = await PostAttachmentAsync();

            TeacherAttachment randomTeacherAttachment = CreateRandomTeacherAttachmentFiller(
                persistedTeacher.Id,
                persistedAttachment.Id).Create();

            return randomTeacherAttachment;
        }

        private async Task<TeacherAttachment> PostTeacherAttachmentAsync()
        {
            TeacherAttachment randomTeacherAttachment = await CreateRandomTeacherAttachment();
            await this.otripleSApiBroker.PostTeacherAttachmentAsync(randomTeacherAttachment);

            return randomTeacherAttachment;
        }

        private async ValueTask<TeacherAttachment> DeleteTeacherAttachmentAsync(TeacherAttachment teacherAttachment)
        {
            TeacherAttachment deletedTeacherAttachment =
                await this.otripleSApiBroker.DeleteTeacherAttachmentByIdsAsync(
                    teacherAttachment.TeacherId,
                    teacherAttachment.AttachmentId);

            await this.otripleSApiBroker.DeleteAttachmentByIdAsync(teacherAttachment.AttachmentId);
            await this.otripleSApiBroker.DeleteTeacherByIdAsync(teacherAttachment.TeacherId);

            return deletedTeacherAttachment;
        }

        private static Teacher CreateRandomTeacher() => CreateRandomTeacherFiller().Create();
        private static Attachment CreateRandomAttachment() => CreateRandomAttachmentFiller().Create();

        private async ValueTask<Teacher> PostTeacherAsync()
        {
            Teacher randomTeacher = CreateRandomTeacher();
            Teacher inputTeacher = randomTeacher;

            return await this.otripleSApiBroker.PostTeacherAsync(inputTeacher);
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

        private static Filler<TeacherAttachment> CreateRandomTeacherAttachmentFiller(
            Guid teacherId,
            Guid attachmentId)
        {
            var now = DateTimeOffset.UtcNow;
            var posterId = Guid.NewGuid();
            var filler = new Filler<TeacherAttachment>();

            filler.Setup()
                .OnProperty(teacherAttachment => teacherAttachment.TeacherId).Use(teacherId)
                .OnProperty(teacherAttachment => teacherAttachment.AttachmentId).Use(attachmentId)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static Filler<Teacher> CreateRandomTeacherFiller()
        {
            var now = DateTimeOffset.UtcNow;
            var posterId = Guid.NewGuid();
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