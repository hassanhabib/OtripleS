// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            CalendarEntryAttachment someCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            var sqlException = GetSqlException();

            var expectedCalendarEntryAttachmentDependencyException =
                new CalendarEntryAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarEntryAttachmentAsync(someCalendarEntryAttachment))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<CalendarEntryAttachment> addCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(someCalendarEntryAttachment);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentDependencyException>(() =>
                addCalendarEntryAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAttachmentAsync(someCalendarEntryAttachment),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCalendarEntryAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            CalendarEntryAttachment someCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            var databaseUpdateException = new DbUpdateException();

            var expectedCalendarEntryAttachmentDependencyException =
                new CalendarEntryAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<CalendarEntryAttachment> addCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(someCalendarEntryAttachment);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentDependencyException>(() =>
                addCalendarEntryAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAttachmentAsync(It.IsAny<CalendarEntryAttachment>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarEntryAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            CalendarEntryAttachment someCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            var serviceException = new Exception();

            var failedCalendarEntryAttachmentServiceException =
                new FailedCalendarEntryAttachmentServiceException(serviceException);

            var expectedCalendarEntryAttachmentServiceException =
                new CalendarEntryAttachmentServiceException(failedCalendarEntryAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarEntryAttachmentAsync(someCalendarEntryAttachment))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<CalendarEntryAttachment> addCalendarEntryAttachmentTask =
                 this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(someCalendarEntryAttachment);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentServiceException>(() =>
                addCalendarEntryAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAttachmentAsync(someCalendarEntryAttachment),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarEntryAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
