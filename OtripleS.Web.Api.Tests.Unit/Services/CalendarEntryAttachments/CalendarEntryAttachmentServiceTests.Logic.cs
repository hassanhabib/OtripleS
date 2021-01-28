// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveCalendarEntryAttachmentById()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            CalendarEntryAttachment storageCalendarEntryAttachment = randomCalendarEntryAttachment;
            CalendarEntryAttachment expectedCalendarEntryAttachment = storageCalendarEntryAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync
                (randomCalendarEntryAttachment.CalendarEntryId, randomCalendarEntryAttachment.AttachmentId))
                    .Returns(new ValueTask<CalendarEntryAttachment>(randomCalendarEntryAttachment));

            // when
            CalendarEntryAttachment actualGuardianAttachment = await
                this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync(
                    randomCalendarEntryAttachment.CalendarEntryId, randomCalendarEntryAttachment.AttachmentId);

            // then
            actualGuardianAttachment.Should().BeEquivalentTo(expectedCalendarEntryAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync
                (randomCalendarEntryAttachment.CalendarEntryId, randomCalendarEntryAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
