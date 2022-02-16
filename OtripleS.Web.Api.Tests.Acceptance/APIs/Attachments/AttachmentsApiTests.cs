// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Attachments;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Attachments
{
    [Collection(nameof(ApiTestCollection))]
    public partial class AttachmentsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public AttachmentsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static Attachment CreateRandomAttachment() =>
            CreateRandomAttachmentFiller().Create();

        private async ValueTask<Attachment> PostRandomAttachmentAsync()
        {
            Attachment randomAttachment = CreateRandomAttachment();
            await this.otripleSApiBroker.PostAttachmentAsync(randomAttachment);

            return randomAttachment;
        }

        private static Attachment UpdateAttachmentRandom(Attachment inputAttachment)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Attachment>();

            filler.Setup()
                .OnProperty(attachment => attachment.Id).Use(inputAttachment.Id)
                .OnProperty(attachment => attachment.CreatedBy).Use(inputAttachment.CreatedBy)
                .OnProperty(attachment => attachment.CreatedDate).Use(inputAttachment.CreatedDate)
                .OnProperty(attachment => attachment.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<Attachment> CreateRandomAttachmentFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid creatorId = Guid.NewGuid();
            var filler = new Filler<Attachment>();

            filler.Setup()
                .OnProperty(attachment => attachment.CreatedBy).Use(creatorId)
                .OnProperty(attachment => attachment.UpdatedBy).Use(creatorId)
                .OnProperty(attachment => attachment.CreatedDate).Use(now)
                .OnProperty(attachment => attachment.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}