// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using OtripleS.Web.Api.Services.CalendarEntryAttachments;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class CalendarEntryAttachmentsController : RESTFulController
    {
        private readonly ICalendarEntryAttachmentService calendarEntryAttachmentService;

        public CalendarEntryAttachmentsController(ICalendarEntryAttachmentService calendarEntryAttachmentService) =>
            this.calendarEntryAttachmentService = calendarEntryAttachmentService;

        [HttpPost]
        public async ValueTask<ActionResult<CalendarEntryAttachment>> PostCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment)
        {
            try
            {
                CalendarEntryAttachment persistedCalendarEntryAttachment =
                    await this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(calendarEntryAttachment);

                return Ok(persistedCalendarEntryAttachment);
            }
            catch (CalendarEntryAttachmentValidationException calendarEntryAttachmentValidationException)
                when (calendarEntryAttachmentValidationException.InnerException is AlreadyExistsCalendarEntryAttachmentException)
            {
                string innerMessage = GetInnerMessage(calendarEntryAttachmentValidationException);

                return Conflict(innerMessage);
            }
            catch (CalendarEntryAttachmentValidationException calendarEntryAttachmentValidationException)
                when (calendarEntryAttachmentValidationException.InnerException is InvalidCalendarEntryAttachmentReferenceException)
            {
                string innerMessage = GetInnerMessage(calendarEntryAttachmentValidationException);

                return FailedDependency(innerMessage);
            }
            catch (CalendarEntryAttachmentValidationException calendarEntryAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(calendarEntryAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (CalendarEntryAttachmentDependencyException calendarEntryAttachmentDependencyException)
            {
                return Problem(calendarEntryAttachmentDependencyException.Message);
            }
            catch (CalendarEntryAttachmentServiceException calendarEntryAttachmentServiceException)
            {
                return Problem(calendarEntryAttachmentServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<CalendarEntryAttachment>> GetAllCalendarEntryAttachments()
        {
            try
            {
                IQueryable storageCalendarEntryAttachments =
                    this.calendarEntryAttachmentService.RetrieveAllCalendarEntryAttachments();

                return Ok(storageCalendarEntryAttachments);
            }
            catch (CalendarEntryAttachmentDependencyException calendarEntryAttachmentDependencyException)
            {
                return Problem(calendarEntryAttachmentDependencyException.Message);
            }
            catch (CalendarEntryAttachmentServiceException calendarEntryAttachmentServiceException)
            {
                return Problem(calendarEntryAttachmentServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
