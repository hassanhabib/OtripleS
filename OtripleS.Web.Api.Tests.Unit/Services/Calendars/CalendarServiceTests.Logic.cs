// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Calendars;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Calendars
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
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldModifyCalendarAsync()
		{
			// given
			DateTimeOffset randomDate = GetRandomDateTime();
			DateTimeOffset randomInputDate = GetRandomDateTime();
			Calendar randomCalendar = CreateRandomCalendar(randomInputDate);
			Calendar inputCalendar = randomCalendar;
			Calendar afterUpdateStorageCalendar = inputCalendar;
			Calendar expectedCalendar = afterUpdateStorageCalendar;
			Calendar beforeUpdateStorageCalendar = randomCalendar.DeepClone();
			inputCalendar.UpdatedDate = randomDate;
			Guid calendarId = inputCalendar.Id;

			this.dateTimeBrokerMock.Setup(broker =>
			   broker.GetCurrentDateTime())
				   .Returns(randomDate);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectCalendarByIdAsync(calendarId))
					.ReturnsAsync(beforeUpdateStorageCalendar);

			this.storageBrokerMock.Setup(broker =>
				broker.UpdateCalendarAsync(inputCalendar))
					.ReturnsAsync(afterUpdateStorageCalendar);

			// when
			Calendar actualCalendar =
				await this.calendarService.ModifyCalendarAsync(inputCalendar);

			// then
			actualCalendar.Should().BeEquivalentTo(expectedCalendar);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(calendarId),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.UpdateCalendarAsync(inputCalendar),
					Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
