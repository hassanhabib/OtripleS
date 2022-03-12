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
        public async Task ShouldAddCalendarAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            randomCalendar.UpdatedBy = randomCalendar.CreatedBy;
            randomCalendar.UpdatedDate = randomCalendar.CreatedDate;
            Calendar inputCalendar = randomCalendar;
            Calendar storageCalendar = randomCalendar;
            Calendar expectedCalendar = randomCalendar;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarAsync(inputCalendar))
                    .ReturnsAsync(storageCalendar);

            // when
            Calendar actualCalendar =
                await this.calendarService.AddCalendarAsync(inputCalendar);

            // then
            actualCalendar.Should().BeEquivalentTo(expectedCalendar);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarAsync(inputCalendar),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
