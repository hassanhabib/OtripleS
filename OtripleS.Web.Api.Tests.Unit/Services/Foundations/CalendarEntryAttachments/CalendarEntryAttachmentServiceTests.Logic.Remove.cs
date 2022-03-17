// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveCalendarEntryAttachmentAsync()
        {
            // given
            var randomCalendarEntryId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputCalendarEntryId = randomCalendarEntryId;
            Guid inputAttachmentId = randomAttachmentId;
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            randomCalendarEntryAttachment.CalendarEntryId = inputCalendarEntryId;
            randomCalendarEntryAttachment.AttachmentId = inputAttachmentId;
            CalendarEntryAttachment storageCalendarEntryAttachment = randomCalendarEntryAttachment;
            CalendarEntryAttachment expectedCalendarEntryAttachment = storageCalendarEntryAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId))
                    .ReturnsAsync(storageCalendarEntryAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteCalendarEntryAttachmentAsync(storageCalendarEntryAttachment))
                    .ReturnsAsync(expectedCalendarEntryAttachment);

            // when
            CalendarEntryAttachment actualCalendarEntryAttachment =
                await this.calendarEntryAttachmentService.RemoveCalendarEntryAttachmentByIdAsync(
                    inputCalendarEntryId, inputAttachmentId);

            // then
            actualCalendarEntryAttachment.Should().BeEquivalentTo(expectedCalendarEntryAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCalendarEntryAttachmentAsync(storageCalendarEntryAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
