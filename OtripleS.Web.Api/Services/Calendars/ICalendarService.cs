// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Calendars;

namespace OtripleS.Web.Api.Services.Calendars
{
	public interface ICalendarService
	{
		ValueTask<Calendar> AddCalendarAsync(Calendar calendar);
		ValueTask<Calendar> RetrieveCalendarByIdAsync(Guid calendarId);
		ValueTask<Calendar> ModifyCalendarAsync(Calendar calendar);
	}
}
