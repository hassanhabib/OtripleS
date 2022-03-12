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
        public void ShouldRetrieveAllCalendars()
        {
            // given
            IQueryable<Calendar> randomCalendars = CreateRandomCalendars();
            IQueryable<Calendar> storageCalendars = randomCalendars;
            IQueryable<Calendar> expectedCalendars = storageCalendars;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendars())
                    .Returns(storageCalendars);

            // when
            IQueryable<Calendar> actualCalendars =
                this.calendarService.RetrieveAllCalendars();

            // then
            actualCalendars.Should().BeEquivalentTo(expectedCalendars);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendars(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
