// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldAddCalendarEntryAttachmentAsync()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            CalendarEntryAttachment inputCalendarEntryAttachment = randomCalendarEntryAttachment;
            CalendarEntryAttachment storageCalendarEntryAttachment = randomCalendarEntryAttachment;
            CalendarEntryAttachment expectedCalendarEntryAttachment = storageCalendarEntryAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarEntryAttachmentAsync(inputCalendarEntryAttachment))
                    .ReturnsAsync(storageCalendarEntryAttachment);

            // when
            CalendarEntryAttachment actualCalendarEntryAttachment =
                await this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(inputCalendarEntryAttachment);

            // then
            actualCalendarEntryAttachment.Should().BeEquivalentTo(expectedCalendarEntryAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAttachmentAsync(inputCalendarEntryAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllCalendarEntryAttachments()
        {
            // given
            IQueryable<CalendarEntryAttachment> randomCalendarEntryAttachments =
                CreateRandomCalendarEntryAttachments();

            IQueryable<CalendarEntryAttachment> storageCalendarEntryAttachments =
                randomCalendarEntryAttachments;

            IQueryable<CalendarEntryAttachment> expectedCalendarEntryAttachments =
                storageCalendarEntryAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendarEntryAttachments())
                    .Returns(storageCalendarEntryAttachments);

            // when
            IQueryable<CalendarEntryAttachment> actualCalendarEntryAttachments =
                this.calendarEntryAttachmentService.RetrieveAllCalendarEntryAttachments();

            // then
            actualCalendarEntryAttachments.Should().BeEquivalentTo(expectedCalendarEntryAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendarEntryAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveCalendarEntryAttachmentById()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            CalendarEntryAttachment storageCalendarEntryAttachment = randomCalendarEntryAttachment;
            CalendarEntryAttachment expectedCalendarEntryAttachment = storageCalendarEntryAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(
                    randomCalendarEntryAttachment.CalendarEntryId, randomCalendarEntryAttachment.AttachmentId))
                        .ReturnsAsync(randomCalendarEntryAttachment);

            // when
            CalendarEntryAttachment actualCalendarEntryAttachment = await
                this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync(
                    randomCalendarEntryAttachment.CalendarEntryId, randomCalendarEntryAttachment.AttachmentId);

            // then
            actualCalendarEntryAttachment.Should().BeEquivalentTo(expectedCalendarEntryAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(
                    randomCalendarEntryAttachment.CalendarEntryId, randomCalendarEntryAttachment.AttachmentId),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
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
