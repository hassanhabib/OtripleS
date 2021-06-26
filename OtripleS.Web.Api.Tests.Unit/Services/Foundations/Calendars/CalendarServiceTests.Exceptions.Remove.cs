// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Models.Calendars.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Calendars
{
    public partial class CalendarServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCalendarId = Guid.NewGuid();
            Guid inputCalendarId = randomCalendarId;
            SqlException sqlException = GetSqlException();

            var expectedCalendarDependencyException =
                new CalendarDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Calendar> deleteCalendarTask =
                this.calendarService.RemoveCalendarByIdAsync(inputCalendarId);

            // then
            await Assert.ThrowsAsync<CalendarDependencyException>(() =>
                deleteCalendarTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCalendarDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCalendarId = Guid.NewGuid();
            Guid inputCalendarId = randomCalendarId;
            var databaseUpdateException = new DbUpdateException();

            var expectedCalendarDependencyException =
                new CalendarDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Calendar> deleteCalendarTask =
                this.calendarService.RemoveCalendarByIdAsync(inputCalendarId);

            // then
            await Assert.ThrowsAsync<CalendarDependencyException>(() =>
                deleteCalendarTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCalendarId = Guid.NewGuid();
            Guid inputCalendarId = randomCalendarId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedCalendarException = new LockedCalendarException(databaseUpdateConcurrencyException);

            var expectedCalendarDependencyException =
                new CalendarDependencyException(lockedCalendarException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Calendar> deleteCalendarTask =
                this.calendarService.RemoveCalendarByIdAsync(inputCalendarId);

            // then
            await Assert.ThrowsAsync<CalendarDependencyException>(() =>
                deleteCalendarTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCalendarId = Guid.NewGuid();
            Guid inputCalendarId = randomCalendarId;
            var exception = new Exception();

            var expectedCalendarServiceException =
                new CalendarServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Calendar> deleteCalendarTask =
                this.calendarService.RemoveCalendarByIdAsync(inputCalendarId);

            // then
            await Assert.ThrowsAsync<CalendarServiceException>(() =>
                deleteCalendarTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}