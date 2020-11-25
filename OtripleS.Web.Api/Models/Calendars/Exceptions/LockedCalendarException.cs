// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Calendars.Exceptions
{
	public class LockedCalendarException : Exception
	{
		public LockedCalendarException(Exception innerException)
			: base("Locked calendar record exception, please try again later.", innerException) { }
	}
}
