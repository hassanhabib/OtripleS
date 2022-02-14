﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCalendarId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var expectedCalendarDependencyException =
                new CalendarDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarByIdAsync(someCalendarId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Calendar> retrieveCalendarByIdTask =
                this.calendarService.RetrieveCalendarByIdAsync(someCalendarId);

            // then
            await Assert.ThrowsAsync<CalendarDependencyException>(() =>
                retrieveCalendarByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCalendarDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarByIdAsync(someCalendarId),
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
            Guid someCalendarId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedCalendarDependencyException =
                new CalendarDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarByIdAsync(someCalendarId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Calendar> retrieveCalendarByIdTask =
                this.calendarService.RetrieveCalendarByIdAsync(someCalendarId);

            // then
            await Assert.ThrowsAsync<CalendarDependencyException>(() =>
                retrieveCalendarByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarByIdAsync(someCalendarId),
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
            Guid someCalendarId = Guid.NewGuid();
            var serviceException = new Exception();

            var expectedCalendarServiceException =
                new CalendarServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarByIdAsync(someCalendarId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Calendar> retrieveCalendarByIdTask =
                this.calendarService.RetrieveCalendarByIdAsync(someCalendarId);

            // then
            await Assert.ThrowsAsync<CalendarServiceException>(() =>
                retrieveCalendarByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarByIdAsync(someCalendarId),
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
