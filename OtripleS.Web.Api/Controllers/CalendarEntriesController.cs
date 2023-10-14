// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;
using OtripleS.Web.Api.Services.Foundations.CalendarEntries;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                CalendarEntry addedCalendarEntry =
                    await this.calendarEntryService.AddCalendarEntryAsync(calendarEntry);

                return Created(addedCalendarEntry);
            }
            catch (CalendarEntryValidationException calendarEntryValidationException)
                when (calendarEntryValidationException.InnerException is AlreadyExistsCalendarEntryException)
            {
                return Conflict(calendarEntryValidationException.GetInnerMessage());
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
                return NotFound(calendarEntryValidationException.GetInnerMessage());
            }
            catch (CalendarEntryValidationException calendarEntryValidationException)
            {
                return BadRequest(calendarEntryValidationException.GetInnerMessage());
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
                return NotFound(calendarEntryValidationException.GetInnerMessage());
            }
            catch (CalendarEntryValidationException calendarEntryValidationException)
            {
                return BadRequest(calendarEntryValidationException.GetInnerMessage());
            }
            catch (CalendarEntryDependencyException calendarEntryDependencyException)
                when (calendarEntryDependencyException.InnerException is LockedCalendarEntryException)
            {
                return Locked(calendarEntryDependencyException.GetInnerMessage());
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
                return NotFound(calendarEntryValidationException.GetInnerMessage());
            }
            catch (CalendarEntryValidationException calendarEntryValidationException)
            {
                return BadRequest(calendarEntryValidationException.Message);
            }
            catch (CalendarEntryDependencyException calendarEntryDependencyException)
                when (calendarEntryDependencyException.InnerException is LockedCalendarEntryException)
            {
                return Locked(calendarEntryDependencyException.GetInnerMessage());
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

    }
}