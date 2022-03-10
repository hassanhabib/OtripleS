// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task ShouldModifyCalendarEntryAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(randomInputDate);
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            CalendarEntry afterUpdateStorageCalendarEntry = inputCalendarEntry;
            CalendarEntry expectedCalendarEntry = afterUpdateStorageCalendarEntry;
            CalendarEntry beforeUpdateStorageCalendarEntry = randomCalendarEntry.DeepClone();
            inputCalendarEntry.UpdatedDate = randomDate;
            Guid calendarEntryId = inputCalendarEntry.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(calendarEntryId))
                    .ReturnsAsync(beforeUpdateStorageCalendarEntry);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateCalendarEntryAsync(inputCalendarEntry))
                    .ReturnsAsync(afterUpdateStorageCalendarEntry);

            // when
            CalendarEntry actualCalendarEntry =
                await this.calendarEntryService.ModifyCalendarEntryAsync(inputCalendarEntry);

            // then
            actualCalendarEntry.Should().BeEquivalentTo(expectedCalendarEntry);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(calendarEntryId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCalendarEntryAsync(inputCalendarEntry),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
