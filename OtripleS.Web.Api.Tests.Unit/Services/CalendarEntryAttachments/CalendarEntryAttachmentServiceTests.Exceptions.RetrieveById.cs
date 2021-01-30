// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid randomCalendarEntryId = Guid.NewGuid();
            Guid someCalendarEntryId = randomCalendarEntryId;
            SqlException sqlException = GetSqlException();

            var expectedCalendarEntryAttachmentDependencyException
                = new CalendarEntryAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<CalendarEntryAttachment> retrieveCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync
                    (someCalendarEntryId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentDependencyException>(() =>
                retrieveCalendarEntryAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomCalendarEntryId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someCalendarEntryId = randomCalendarEntryId;
            var databaseUpdateException = new DbUpdateException();

            var expectedCalendarEntryAttachmentDependencyException =
                new CalendarEntryAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<CalendarEntryAttachment> retrieveAttachmentTask =
                this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync
                    (someCalendarEntryId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentDependencyException>(() => 
                retrieveAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomCalendarEntryId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputCalendarEntryId = randomCalendarEntryId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedCalendarEntryAttachmentException =
                new LockedCalendarEntryAttachmentException(databaseUpdateConcurrencyException);

            var expectedCalendarEntryAttachmentException =
                new CalendarEntryAttachmentDependencyException(lockedCalendarEntryAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<CalendarEntryAttachment> retrieveCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentDependencyException>(() =>
                retrieveCalendarEntryAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomCalendarEntryId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputCalendarEntryId = randomCalendarEntryId;
            var exception = new Exception();

            var expectedCalendarEntryAttachmentException =
                new CalendarEntryAttachmentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<CalendarEntryAttachment> retrieveCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync
                    (inputCalendarEntryId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentServiceException>(() =>
                retrieveCalendarEntryAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(inputCalendarEntryId, inputAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
