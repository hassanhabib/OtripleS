﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            var randomAttachmentId = Guid.NewGuid();
            var randomCalendarEntryId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someCalendarEntryId = randomCalendarEntryId;
            SqlException sqlException = GetSqlException();

            var expectedCalendarEntryAttachmentDependencyException
                = new CalendarEntryAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<CalendarEntryAttachment> removeCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RemoveCalendarEntryAttachmentByIdAsync(
                    someCalendarEntryId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentDependencyException>(() =>
                removeCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            var randomAttachmentId = Guid.NewGuid();
            var randomCalendarEntryId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someCalendarEntryId = randomCalendarEntryId;
            var databaseUpdateException = new DbUpdateException();

            var expectedCalendarEntryAttachmentDependencyException =
                new CalendarEntryAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<CalendarEntryAttachment> removeCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RemoveCalendarEntryAttachmentByIdAsync
                (someCalendarEntryId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentDependencyException>(
                () => removeCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            var randomAttachmentId = Guid.NewGuid();
            var randomCalendarEntryId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someCalendarEntryId = randomCalendarEntryId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedCalendarEntryAttachmentException(databaseUpdateConcurrencyException);

            var expectedCalendarEntryAttachmentException =
                new CalendarEntryAttachmentDependencyException(lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<CalendarEntryAttachment> removeCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RemoveCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentDependencyException>(() =>
                removeCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            var randomAttachmentId = Guid.NewGuid();
            var randomCalendarEntryId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someCalendarEntryId = randomCalendarEntryId;
            var exception = new Exception();

            var expectedCalendarEntryAttachmentException =
                new CalendarEntryAttachmentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<CalendarEntryAttachment> removeCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RemoveCalendarEntryAttachmentByIdAsync(
                    someCalendarEntryId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentServiceException>(() =>
                removeCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}