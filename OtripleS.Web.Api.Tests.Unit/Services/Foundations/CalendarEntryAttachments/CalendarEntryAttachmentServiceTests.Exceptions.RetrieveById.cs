// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someCalendarEntryId = Guid.NewGuid();
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
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCalendarEntryAttachmentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someCalendarEntryId = Guid.NewGuid();
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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarEntryAttachmentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someCalendarEntryId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedCalendarEntryAttachmentException =
                new LockedCalendarEntryAttachmentException(databaseUpdateConcurrencyException);

            var expectedCalendarEntryAttachmentException =
                new CalendarEntryAttachmentDependencyException(lockedCalendarEntryAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<CalendarEntryAttachment> retrieveCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentDependencyException>(() =>
                retrieveCalendarEntryAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarEntryAttachmentException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someCalendarEntryId = Guid.NewGuid();
            var serviceException = new Exception();

            var expectedCalendarEntryAttachmentException =
                new CalendarEntryAttachmentServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<CalendarEntryAttachment> retrieveCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync
                    (someCalendarEntryId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentServiceException>(() =>
                retrieveCalendarEntryAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryAttachmentByIdAsync(someCalendarEntryId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarEntryAttachmentException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
