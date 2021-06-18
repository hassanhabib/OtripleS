// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Foundations.Calendars;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Calendars
{
    public partial class CalendarServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllIfCalendarsIsEmptyAndLogIt()
        {
            // given
            IQueryable<Calendar> emptyStorageCalendars = new List<Calendar>().AsQueryable();
            IQueryable<Calendar> expectedCalendars = emptyStorageCalendars;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendars())
                    .Returns(expectedCalendars);

            // when
            IQueryable<Calendar> actualCalendars =
                this.calendarService.RetrieveAllCalendars();

            // then
            actualCalendars.Should().BeEquivalentTo(emptyStorageCalendars);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendars(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No calendars found in storage."),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
