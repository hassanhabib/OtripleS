// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Calendars;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Calendars
{
    public partial class CalendarServiceTests
    {
        [Fact]
        public async Task ShouldDeleteCalendarByIdAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Guid inputCalendarId = randomCalendar.Id;
            Calendar inputCalendar = randomCalendar;
            Calendar storageCalendar = randomCalendar;
            Calendar expectedCalendar = randomCalendar;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId))
                    .ReturnsAsync(inputCalendar);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteCalendarAsync(inputCalendar))
                    .ReturnsAsync(storageCalendar);

            // when
            Calendar actualCalendar =
                await this.calendarService.RemoveCalendarByIdAsync(inputCalendarId);

            // then
            actualCalendar.Should().BeEquivalentTo(expectedCalendar);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCalendarAsync(inputCalendar),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
