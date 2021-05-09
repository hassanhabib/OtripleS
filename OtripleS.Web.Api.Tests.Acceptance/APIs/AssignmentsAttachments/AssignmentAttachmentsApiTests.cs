// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Assignments;
using OtripleS.Web.Api.Tests.Acceptance.Models.AssignmentsAttachments;
using OtripleS.Web.Api.Tests.Acceptance.Models.Attachments;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.AssignmentsAttachments
{
    [Collection(nameof(ApiTestCollection))]
    public partial class AssignmentAttachmentsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public AssignmentAttachmentsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private async Task<AssignmentAttachment> CreateRandomAssignmentAttachment()
        {
            Assignment persistedAssignment = await PostAssignmentAsync();
            Attachment persistedAttachment = await PostAttachmentAsync();

            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachmentFiller(
                persistedAssignment.Id,
                persistedAttachment.Id).Create();

            return randomAssignmentAttachment;
        }

        private async Task<AssignmentAttachment> PostAssignmentAttachmentAsync()
        {
            AssignmentAttachment randomAssignmentAttachment = await CreateRandomAssignmentAttachment();
            await this.otripleSApiBroker.PostAssignmentAttachmentAsync(randomAssignmentAttachment);

            return randomAssignmentAttachment;
        }

        private async ValueTask<AssignmentAttachment> DeleteAssignmentAttachmentAsync(AssignmentAttachment assignmentAttachment)
        {
            AssignmentAttachment deletedAssignmentAttachment =
                await this.otripleSApiBroker.DeleteAssignmentAttachmentByIdsAsync(
                    assignmentAttachment.AssignmentId,
                    assignmentAttachment.AttachmentId);

            await this.otripleSApiBroker.DeleteAttachmentByIdAsync(assignmentAttachment.AttachmentId);

            Assignment deletedAssignment =
                await this.otripleSApiBroker.DeleteAssignmentByIdAsync(assignmentAttachment.AssignmentId);

            return deletedAssignmentAttachment;
        }

        private Attachment CreateRandomAttachment() => CreateRandomAttachmentFiller().Create();

        private async ValueTask<Assignment> PostAssignmentAsync()
        {
            Assignment randomAssignment = CreateRandomAssignment();
            Assignment inputAssignment = randomAssignment;

            return await this.otripleSApiBroker.PostAssignmentAsync(inputAssignment);
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

        private Filler<AssignmentAttachment> CreateRandomAssignmentAttachmentFiller(Guid assignmentId, Guid attachmentId)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<AssignmentAttachment>();

            filler.Setup()
                .OnProperty(assignmentAttachment => assignmentAttachment.AssignmentId).Use(assignmentId)
                .OnProperty(assignmentAttachment => assignmentAttachment.AttachmentId).Use(attachmentId)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private Assignment CreateRandomAssignment()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<Assignment>();

            filler.Setup()
                .OnProperty(assignment => assignment.CreatedBy).Use(posterId)
                .OnProperty(assignment => assignment.UpdatedBy).Use(posterId)
                .OnProperty(assignment => assignment.CreatedDate).Use(now)
                .OnProperty(assignment => assignment.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private Filler<Attachment> CreateRandomAttachmentFiller()
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