// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Models.Calendars.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Calendars
{
	public partial class CalendarServiceTests
	{
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCalendarIsNullAndLogItAsync()
        {
            // given
            Calendar randomCalendar = null;
            Calendar nullCalendar = randomCalendar;

            var nullCalendarException = new NullCalendarException();

            var expectedCalendarValidationException =
                new CalendarValidationException(nullCalendarException);

            // when
            ValueTask<Calendar> registerCalendarTask =
                this.calendarService.AddCalendarAsync(nullCalendar);

            // then
            await Assert.ThrowsAsync<CalendarValidationException>(() =>
                registerCalendarTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarAsync(It.IsAny<Calendar>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
