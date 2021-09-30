// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;
using OtripleS.Web.Api.Services.Foundations.CalendarEntries;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarEntriesController : RESTFulController
    {
        private readonly ICalendarEntryService calendarEntryService;

        public CalendarEntriesController(ICalendarEntryService calendarEntryService) =>
            this.calendarEntryService = calendarEntryService;

        [HttpPost]
        public async ValueTask<ActionResult<CalendarEntry>> PostCalendarAsync(CalendarEntry calendarEntry)
        {
            try
            {
                CalendarEntry persistedCalendarEntry =
                    await this.calendarEntryService.AddCalendarEntryAsync(calendarEntry);

                return Created(persistedCalendarEntry);
            }
            catch (CalendarEntryValidationException calendarEntryValidationException)
                when (calendarEntryValidationException.InnerException is AlreadyExistsCalendarEntryException)
            {
                string innerMessage = GetInnerMessage(calendarEntryValidationException);

                return Conflict(innerMessage);
            }
            catch (CalendarEntryValidationException calendarEntryValidationException)
            {
                return BadRequest(calendarEntryValidationException.InnerException);
            }
            catch (CalendarEntryDependencyException calendarEntryDependencyException)
            {
                return Problem(calendarEntryDependencyException.Message);
            }
            catch (CalendarEntryServiceException calendarEntryServiceException)
            {
                return Problem(calendarEntryServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<CalendarEntry>> GetAllCalendarEntries()
        {
            try
            {
                IQueryable storageCalendarEntries =
                    this.calendarEntryService.RetrieveAllCalendarEntries();

                return Ok(storageCalendarEntries);
            }
            catch (CalendarEntryDependencyException calendarEntryDependencyException)
            {
                return Problem(calendarEntryDependencyException.Message);
            }
            catch (CalendarEntryServiceException calendarEntryServiceException)
            {
                return Problem(calendarEntryServiceException.Message);
            }
        }

        [HttpGet("{calendarEntryId}")]
        public async ValueTask<ActionResult<CalendarEntry>> GetCalendarEntryAsync(Guid calendarEntryId)
        {
            try
            {
                CalendarEntry storageCalendarEntry =
                    await this.calendarEntryService.RetrieveCalendarEntryByIdAsync(calendarEntryId);

                return Ok(storageCalendarEntry);
            }
            catch (CalendarEntryValidationException calendarEntryValidationException)
                when (calendarEntryValidationException.InnerException is NotFoundCalendarEntryException)
            {
                string innerMessage = GetInnerMessage(calendarEntryValidationException);

                return NotFound(innerMessage);
            }
            catch (CalendarEntryValidationException calendarEntryValidationException)
            {
                string innerMessage = GetInnerMessage(calendarEntryValidationException);

                return BadRequest(innerMessage);
            }
            catch (CalendarEntryDependencyException calendarEntryDependencyException)
            {
                return Problem(calendarEntryDependencyException.Message);
            }
            catch (CalendarEntryServiceException calendarEntryServiceException)
            {
                return Problem(calendarEntryServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<CalendarEntry>> PutCalendarAsync(CalendarEntry calendarEntry)
        {
            try
            {
                CalendarEntry registeredCalendarEntry =
                    await this.calendarEntryService.ModifyCalendarEntryAsync(calendarEntry);

                return Ok(registeredCalendarEntry);
            }
            catch (CalendarEntryValidationException calendarEntryValidationException)
                when (calendarEntryValidationException.InnerException is NotFoundCalendarEntryException)
            {
                string innerMessage = GetInnerMessage(calendarEntryValidationException);

                return NotFound(innerMessage);
            }
            catch (CalendarEntryValidationException calendarEntryValidationException)
            {
                string innerMessage = GetInnerMessage(calendarEntryValidationException);

                return BadRequest(innerMessage);
            }
            catch (CalendarEntryDependencyException calendarEntryDependencyException)
                when (calendarEntryDependencyException.InnerException is LockedCalendarEntryException)
            {
                string innerMessage = GetInnerMessage(calendarEntryDependencyException);

                return Locked(innerMessage);
            }
            catch (CalendarEntryDependencyException calendarEntryDependencyException)
            {
                return Problem(calendarEntryDependencyException.Message);
            }
            catch (CalendarEntryServiceException calendarEntryServiceException)
            {
                return Problem(calendarEntryServiceException.Message);
            }
        }

        [HttpDelete("{calendarEntryId}")]
        public async ValueTask<ActionResult<CalendarEntry>> DeleteCalendarAsync(Guid calendarEntryId)
        {
            try
            {
                CalendarEntry storageCalendarEntry =
                    await this.calendarEntryService.RemoveCalendarEntryByIdAsync(calendarEntryId);

                return Ok(storageCalendarEntry);
            }
            catch (CalendarEntryValidationException calendarEntryValidationException)
                when (calendarEntryValidationException.InnerException is NotFoundCalendarEntryException)
            {
                string innerMessage = GetInnerMessage(calendarEntryValidationException);

                return NotFound(innerMessage);
            }
            catch (CalendarEntryValidationException calendarEntryValidationException)
            {
                return BadRequest(calendarEntryValidationException.Message);
            }
            catch (CalendarEntryDependencyException calendarEntryDependencyException)
                when (calendarEntryDependencyException.InnerException is LockedCalendarEntryException)
            {
                string innerMessage = GetInnerMessage(calendarEntryDependencyException);

                return Locked(innerMessage);
            }
            catch (CalendarEntryDependencyException calendarEntryDependencyException)
            {
                return Problem(calendarEntryDependencyException.Message);
            }
            catch (CalendarEntryServiceException calendarEntryServiceException)
            {
                return Problem(calendarEntryServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}