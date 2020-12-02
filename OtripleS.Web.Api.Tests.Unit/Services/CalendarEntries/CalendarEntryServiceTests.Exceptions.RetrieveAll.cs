// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntries
{
    public partial class CalendarEntryServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllCalendarEntriesWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedCalendarEntryDependencyException =
                new CalendarEntryDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendarEntries())
                    .Throws(sqlException);

            // when . then
            Assert.Throws<CalendarEntryDependencyException>(() =>
                this.calendarEntryService.RetrieveAllCalendarEntries());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCalendarEntryDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendarEntries(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllCalendarEntriesWhenDbExceptionOccursAndLogIt()
        {
            // given
            var databaseUpdateException = new DbUpdateException();

            var expectedCalendarEntryDependencyException =
                new CalendarEntryDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendarEntries())
                    .Throws(databaseUpdateException);

            // when . then
            Assert.Throws<CalendarEntryDependencyException>(() =>
                this.calendarEntryService.RetrieveAllCalendarEntries());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendarEntries(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllCalendarEntriesWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedCalendarEntryServiceException =
                new CalendarEntryServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendarEntries())
                    .Throws(exception);

            // when . then
            Assert.Throws<CalendarEntryServiceException>(() =>
                this.calendarEntryService.RetrieveAllCalendarEntries());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendarEntries(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
