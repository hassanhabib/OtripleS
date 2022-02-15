﻿//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenIdsAreInvalidAndLogItAsync()
        {
            // given
            Guid invalidCalendarEntryId = Guid.Empty;
            Guid invalidAttachmentId = Guid.Empty;

            var invalidCalendarEntryAttachmentInputException =
                new InvalidCalendarEntryAttachmentException();

            invalidCalendarEntryAttachmentInputException.AddData(
                key: nameof(CalendarEntryAttachment.AttachmentId),
                values: "Id is required");

            invalidCalendarEntryAttachmentInputException.AddData(
                key: nameof(CalendarEntryAttachment.CalendarEntryId),
                values: "Id is required");

            var expectedCalendarEntryAttachmentValidationException =
                new CalendarEntryAttachmentValidationException(
                    invalidCalendarEntryAttachmentInputException);

            // when
            ValueTask<CalendarEntryAttachment> removeCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RemoveCalendarEntryAttachmentByIdAsync(
                    invalidCalendarEntryId,
                    invalidAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentValidationException>(() =>
                removeCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedCalendarEntryAttachmentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCalendarEntryAttachmentAsync(
                    It.IsAny<CalendarEntryAttachment>()),
                        Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowValidationExceptionOnRemoveWhenStorageCalendarEntryAttachmentIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment(randomDateTime);
            Guid inputAttachmentId = randomCalendarEntryAttachment.AttachmentId;
            Guid inputCalendarEntryId = randomCalendarEntryAttachment.CalendarEntryId;
            CalendarEntryAttachment nullStorageCalendarEntryAttachment = null;

            var notFoundCalendarEntryAttachmentException =
                new NotFoundCalendarEntryAttachmentException(inputCalendarEntryId, inputAttachmentId);

            var expectedCalendarEntryValidationException =
                new CalendarEntryAttachmentValidationException(notFoundCalendarEntryAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId))
                    .ReturnsAsync(nullStorageCalendarEntryAttachment);

            // when
            ValueTask<CalendarEntryAttachment> removeCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RemoveCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentValidationException>(() =>
                removeCalendarEntryAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarEntryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}