﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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

        [Fact]
        public async Task ShouldRetrieveCalendarEntryByIdAsync()
        {
            // given
            Guid randomCalendarEntryId = Guid.NewGuid();
            Guid inputCalendarEntryId = randomCalendarEntryId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(randomDateTime);
            CalendarEntry storageCalendarEntry = randomCalendarEntry;
            CalendarEntry expectedCalendarEntry = storageCalendarEntry;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId))
                    .ReturnsAsync(storageCalendarEntry);

            // when
            CalendarEntry actualCalendarEntry =
                await this.calendarEntryService.RetrieveCalendarEntryByIdAsync(
                    inputCalendarEntryId);

            // then
            actualCalendarEntry.Should().BeEquivalentTo(expectedCalendarEntry);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }        

        [Fact]
        public async Task ShouldDeleteCalendarEntryByIdAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            Guid inputCalendarEntryId = randomCalendarEntry.Id;
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            CalendarEntry storageCalendarEntry = randomCalendarEntry;
            CalendarEntry expectedCalendarEntry = randomCalendarEntry;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId))
                    .ReturnsAsync(inputCalendarEntry);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteCalendarEntryAsync(inputCalendarEntry))
                    .ReturnsAsync(storageCalendarEntry);

            // when
            CalendarEntry actualCalendarEntry =
                await this.calendarEntryService.RemoveCalendarEntryByIdAsync(inputCalendarEntryId);

            // then
            actualCalendarEntry.Should().BeEquivalentTo(expectedCalendarEntry);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCalendarEntryAsync(inputCalendarEntry),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
