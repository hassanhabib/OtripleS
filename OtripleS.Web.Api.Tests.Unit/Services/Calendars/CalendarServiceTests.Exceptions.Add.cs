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

namespace OtripleS.Web.Api.Tests.Unit.Services.Calendars
{
	public partial class CalendarServiceTests
	{
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Calendar inputCalendar = randomCalendar;
            inputCalendar.UpdatedBy = inputCalendar.CreatedBy;
            var sqlException = GetSqlException();

            var expectedCalendarDependencyException =
                new CalendarDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarAsync(inputCalendar))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Calendar> registerCalendarTask =
                this.calendarService.AddCalendarAsync(inputCalendar);

            // then
            await Assert.ThrowsAsync<CalendarDependencyException>(() =>
                registerCalendarTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCalendarDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarAsync(inputCalendar),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Calendar inputCalendar = randomCalendar;
            inputCalendar.UpdatedBy = inputCalendar.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedCalendarDependencyException =
                new CalendarDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarAsync(inputCalendar))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Calendar> registerCalendarTask =
                this.calendarService.AddCalendarAsync(inputCalendar);

            // then
            await Assert.ThrowsAsync<CalendarDependencyException>(() =>
                registerCalendarTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarAsync(inputCalendar),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Calendar inputCalendar = randomCalendar;
            inputCalendar.UpdatedBy = inputCalendar.CreatedBy;
            var exception = new Exception();

            var expectedCalendarServiceException =
                new CalendarServiceException(exception);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarAsync(inputCalendar))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Calendar> registerCalendarTask =
                 this.calendarService.AddCalendarAsync(inputCalendar);

            // then
            await Assert.ThrowsAsync<CalendarServiceException>(() =>
                registerCalendarTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarAsync(inputCalendar),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
