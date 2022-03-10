﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntries;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntries
{
    public partial class CalendarEntryServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllCalendarEntries()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<CalendarEntry> randomCalendarEntries = CreateRandomCalendarEntries(randomDateTime);
            IQueryable<CalendarEntry> storageCalendarEntries = randomCalendarEntries;
            IQueryable<CalendarEntry> expectedCalendarEntries = storageCalendarEntries;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendarEntries())
                    .Returns(storageCalendarEntries);

            // when
            IQueryable<CalendarEntry> actualCalendarEntries =
                this.calendarEntryService.RetrieveAllCalendarEntries();

            // then
            actualCalendarEntries.Should().BeEquivalentTo(expectedCalendarEntries);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendarEntries(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
