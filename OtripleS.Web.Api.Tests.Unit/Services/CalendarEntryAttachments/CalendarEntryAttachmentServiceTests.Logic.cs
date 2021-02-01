// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

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
            CalendarEntryAttachment actualCalendarEntryAttachment = await
                this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync(
                    randomCalendarEntryAttachment.CalendarEntryId, randomCalendarEntryAttachment.AttachmentId);

            // then
            actualCalendarEntryAttachment.Should().BeEquivalentTo(expectedCalendarEntryAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync
                (randomCalendarEntryAttachment.CalendarEntryId, randomCalendarEntryAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllCalendarEntryAttachments()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<CalendarEntryAttachment> randomCalendarEntryAttachment = CreateRandomCalendarEntries(randomDateTime);
            IQueryable<CalendarEntryAttachment> storageCalendarEntryAttachment = randomCalendarEntryAttachment;
            IQueryable<CalendarEntryAttachment> expectedCalendarEntryAttachment = storageCalendarEntryAttachment;

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectAllCalendarEntryAttachments())
                .Returns(storageCalendarEntryAttachment);

            // when
            IQueryable<CalendarEntryAttachment> actualCalendarEntryAttachments =
                this.calendarEntryAttachmentService.RetrieveAllCalendarEntryAttachments();

            //then
            actualCalendarEntryAttachments.Should().BeEquivalentTo(expectedCalendarEntryAttachment);

            this.dateTimeBrokerMock.Verify(broker =>
                    broker.GetCurrentDateTime(),
                Times.Never);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectAllCalendarEntryAttachments(),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

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
