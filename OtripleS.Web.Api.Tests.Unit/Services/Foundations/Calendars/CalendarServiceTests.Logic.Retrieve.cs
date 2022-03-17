// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Calendars;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Calendars
{
    public partial class CalendarServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveCalendarByIdAsync()
        {
            // given
            Guid randomCalendarId = Guid.NewGuid();
            Guid inputCalendarId = randomCalendarId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(randomDateTime);
            Calendar storageCalendar = randomCalendar;
            Calendar expectedCalendar = storageCalendar;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId))
                    .ReturnsAsync(storageCalendar);

            // when
            Calendar actualCalendar =
                await this.calendarService.RetrieveCalendarByIdAsync(inputCalendarId);

            // then
            actualCalendar.Should().BeEquivalentTo(expectedCalendar);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
