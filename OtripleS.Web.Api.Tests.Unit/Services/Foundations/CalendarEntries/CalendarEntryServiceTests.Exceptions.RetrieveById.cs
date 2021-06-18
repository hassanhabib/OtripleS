// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Foundations.CalendarEntries;
using OtripleS.Web.Api.Models.Foundations.CalendarEntries.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntries
{
    public partial class CalendarEntryServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCalendarEntryId = Guid.NewGuid();
            Guid inputCalendarEntryId = randomCalendarEntryId;
            var sqlException = GetSqlException();

            var expectedCalendarEntryDependencyException =
                new CalendarEntryDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<CalendarEntry> retrieveCalendarEntryByIdTask =
                this.calendarEntryService.RetrieveCalendarEntryByIdAsync(inputCalendarEntryId);

            // then
            await Assert.ThrowsAsync<CalendarEntryDependencyException>(() =>
                retrieveCalendarEntryByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCalendarEntryDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCalendarEntryId = Guid.NewGuid();
            Guid inputCalendarEntryId = randomCalendarEntryId;
            var databaseUpdateException = new DbUpdateException();

            var expectedCalendarEntryDependencyException =
                new CalendarEntryDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<CalendarEntry> retrieveCalendarEntryByIdTask =
                this.calendarEntryService.RetrieveCalendarEntryByIdAsync(inputCalendarEntryId);

            // then
            await Assert.ThrowsAsync<CalendarEntryDependencyException>(() =>
                retrieveCalendarEntryByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCalendarEntryId = Guid.NewGuid();
            Guid inputCalendarEntryId = randomCalendarEntryId;
            var exception = new Exception();

            var expectedCalendarEntryServiceException =
                new CalendarEntryServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<CalendarEntry> retrieveCalendarEntryByIdTask =
                this.calendarEntryService.RetrieveCalendarEntryByIdAsync(inputCalendarEntryId);

            // then
            await Assert.ThrowsAsync<CalendarEntryServiceException>(() =>
                retrieveCalendarEntryByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
