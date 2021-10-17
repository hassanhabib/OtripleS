// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Models.Calendars.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Calendars;
using RESTFulSense.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarsController : RESTFulController
    {
        private readonly ICalendarService calendarService;

        public CalendarsController(ICalendarService calendarService) =>
            this.calendarService = calendarService;

        [HttpPost]
        public async ValueTask<ActionResult<Calendar>> PostCalendarAsync(Calendar calendar)
        {
            try
            {
                Calendar persistedCalendar =
                    await this.calendarService.AddCalendarAsync(calendar);

                return Created(persistedCalendar);
            }
            catch (CalendarValidationException calendarValidationException)
                when (calendarValidationException.InnerException is AlreadyExistsCalendarException)
            {
                string innerMessage = GetInnerMessage(calendarValidationException);

                return Conflict(innerMessage);
            }
            catch (CalendarValidationException calendarValidationException)
            {
                string innerMessage = GetInnerMessage(calendarValidationException);

                return BadRequest(innerMessage);
            }
            catch (CalendarDependencyException calendarDependencyException)
            {
                return Problem(calendarDependencyException.Message);
            }
            catch (CalendarServiceException calendarServiceException)
            {
                return Problem(calendarServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Calendar>> GetAllCalendars()
        {
            try
            {
                IQueryable storageCalendars =
                    this.calendarService.RetrieveAllCalendars();

                return Ok(storageCalendars);
            }
            catch (CalendarDependencyException calendarDependencyException)
            {
                return Problem(calendarDependencyException.Message);
            }
            catch (CalendarServiceException calendarServiceException)
            {
                return Problem(calendarServiceException.Message);
            }
        }

        [HttpGet("{calendarId}")]
        public async ValueTask<ActionResult<Calendar>> GetCalendarAsync(Guid calendarId)
        {
            try
            {
                Calendar storageCalendar =
                    await this.calendarService.RetrieveCalendarByIdAsync(calendarId);

                return Ok(storageCalendar);
            }
            catch (CalendarValidationException calendarValidationException)
                when (calendarValidationException.InnerException is NotFoundCalendarException)
            {
                string innerMessage = GetInnerMessage(calendarValidationException);

                return NotFound(innerMessage);
            }
            catch (CalendarValidationException calendarValidationException)
            {
                string innerMessage = GetInnerMessage(calendarValidationException);

                return BadRequest(innerMessage);
            }
            catch (CalendarDependencyException calendarDependencyException)
            {
                return Problem(calendarDependencyException.Message);
            }
            catch (CalendarServiceException calendarServiceException)
            {
                return Problem(calendarServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Calendar>> PutCalendarAsync(Calendar calendar)
        {
            try
            {
                Calendar registeredCalendar =
                    await this.calendarService.ModifyCalendarAsync(calendar);

                return Ok(registeredCalendar);
            }
            catch (CalendarValidationException calendarValidationException)
                when (calendarValidationException.InnerException is NotFoundCalendarException)
            {
                string innerMessage = GetInnerMessage(calendarValidationException);

                return NotFound(innerMessage);
            }
            catch (CalendarValidationException calendarValidationException)
            {
                string innerMessage = GetInnerMessage(calendarValidationException);

                return BadRequest(innerMessage);
            }
            catch (CalendarDependencyException calendarDependencyException)
                when (calendarDependencyException.InnerException is LockedCalendarException)
            {
                string innerMessage = GetInnerMessage(calendarDependencyException);

                return Locked(innerMessage);
            }
            catch (CalendarDependencyException calendarDependencyException)
            {
                return Problem(calendarDependencyException.Message);
            }
            catch (CalendarServiceException calendarServiceException)
            {
                return Problem(calendarServiceException.Message);
            }
        }

        [HttpDelete("{calendarId}")]
        public async ValueTask<ActionResult<Calendar>> DeleteCalendarAsync(Guid calendarId)
        {
            try
            {
                Calendar storageCalendar =
                    await this.calendarService.RemoveCalendarByIdAsync(calendarId);

                return Ok(storageCalendar);
            }
            catch (CalendarValidationException calendarValidationException)
                when (calendarValidationException.InnerException is NotFoundCalendarException)
            {
                string innerMessage = GetInnerMessage(calendarValidationException);

                return NotFound(innerMessage);
            }
            catch (CalendarValidationException calendarValidationException)
            {
                return BadRequest(calendarValidationException.Message);
            }
            catch (CalendarDependencyException calendarDependencyException)
                when (calendarDependencyException.InnerException is LockedCalendarException)
            {
                string innerMessage = GetInnerMessage(calendarDependencyException);

                return Locked(innerMessage);
            }
            catch (CalendarDependencyException calendarDependencyException)
            {
                return Problem(calendarDependencyException.Message);
            }
            catch (CalendarServiceException calendarServiceException)
            {
                return Problem(calendarServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}