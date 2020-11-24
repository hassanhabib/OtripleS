// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Models.Calendars.Exceptions;

namespace OtripleS.Web.Api.Services.Calendars
{
	public partial class CalendarService
	{
		private void ValidateCalendarOnModify(Calendar calendar)
		{
			ValidateCalendarIsNull(calendar);
			ValidateCalendarIdIsNull(calendar.Id);
			ValidateCalendarFields(calendar);
		}

		private void ValidateCalendarIsNull(Calendar calendar)
		{
			if (calendar is null)
			{
				throw new NullCalendarException();
			}
		}

		private void ValidateCalendarIdIsNull(Guid calendarId)
		{
			if (calendarId == default)
			{
				throw new InvalidCalendarInputException(
					parameterName: nameof(Calendar.Id),
					parameterValue: calendarId);
			}
		}

		private void ValidateCalendarFields(Calendar calendar)
		{
			if (IsInvalid(calendar.Label))
			{
				throw new InvalidCalendarInputException(
					parameterName: nameof(Calendar.Label),
					parameterValue: calendar.Label);
			}
		}

		private void ValidateCalendarId(Guid calendarId)
		{
			if (calendarId == Guid.Empty)
			{
				throw new InvalidCalendarInputException(
					parameterName: nameof(Calendar.Id),
					parameterValue: calendarId);
			}
		}

		private static void ValidateStorageCalendar(Calendar storageCalendar, Guid calendarId)
		{
			if (storageCalendar == null)
			{
				throw new NotFoundCalendarException(calendarId);
			}
		}

		private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
	}
}
