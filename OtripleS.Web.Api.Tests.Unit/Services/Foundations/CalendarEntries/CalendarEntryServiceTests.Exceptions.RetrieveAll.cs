// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntries
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
            
            // when
            Action retrieveAllCalendarEntryAction = () =>
                this.calendarEntryService.RetrieveAllCalendarEntries();
           
            // then
            Assert.Throws<CalendarEntryDependencyException>(
                retrieveAllCalendarEntryAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCalendarEntryDependencyException))),
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
            var serviceException = new Exception();

            var expectedCalendarEntryServiceException =
                new CalendarEntryServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendarEntries())
                    .Throws(serviceException);

            // when
            Action retrieveAllCalendarEntryAction = () =>
                this.calendarEntryService.RetrieveAllCalendarEntries();

            // then
            Assert.Throws<CalendarEntryServiceException>(
                retrieveAllCalendarEntryAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarEntryServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendarEntries(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
