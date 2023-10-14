// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using OtripleS.Web.Api.Services.Foundations.CalendarEntryAttachments;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarEntriesAttachmentsController : RESTFulController
    {
        private readonly ICalendarEntryAttachmentService calendarEntryAttachmentService;

        public CalendarEntriesAttachmentsController(ICalendarEntryAttachmentService calendarEntryAttachmentService) =>
            this.calendarEntryAttachmentService = calendarEntryAttachmentService;

        [HttpPost]
        public async ValueTask<ActionResult<CalendarEntryAttachment>> PostCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment)
        {
            try
            {
                CalendarEntryAttachment addedCalendarEntryAttachment =
                    await this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(calendarEntryAttachment);

                return Created(addedCalendarEntryAttachment);
            }
            catch (CalendarEntryAttachmentValidationException calendarEntryAttachmentValidationException)
                when (calendarEntryAttachmentValidationException.InnerException is AlreadyExistsCalendarEntryAttachmentException)
            {
                return Conflict(calendarEntryAttachmentValidationException.GetInnerMessage());
            }
            catch (CalendarEntryAttachmentValidationException calendarEntryAttachmentValidationException)
                when (calendarEntryAttachmentValidationException.InnerException is InvalidCalendarEntryAttachmentReferenceException)
            {
                return FailedDependency(calendarEntryAttachmentValidationException.GetInnerMessage());
            }
            catch (CalendarEntryAttachmentValidationException calendarEntryAttachmentValidationException)
            {
                return BadRequest(calendarEntryAttachmentValidationException.GetInnerMessage());
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

        [HttpGet("calendarentries/{calendarEntryId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<CalendarEntryAttachment>> GetCalendarEntryAttachmentAsync(
            Guid calendarEntryId,
            Guid attachmentId)
        {
            try
            {
                CalendarEntryAttachment storageCalendarEntryAttachment =
                    await this.calendarEntryAttachmentService.RetrieveCalendarEntryAttachmentByIdAsync(calendarEntryId, attachmentId);

                return Ok(storageCalendarEntryAttachment);
            }
            catch (CalendarEntryAttachmentValidationException semesterCalendarEntryAttachmentValidationException)
                when (semesterCalendarEntryAttachmentValidationException.InnerException is NotFoundCalendarEntryAttachmentException)
            {
                return NotFound(semesterCalendarEntryAttachmentValidationException.GetInnerMessage());
            }
            catch (CalendarEntryAttachmentValidationException semesterCalendarEntryAttachmentValidationException)
            {
                return BadRequest(semesterCalendarEntryAttachmentValidationException.GetInnerMessage());
            }
            catch (CalendarEntryAttachmentDependencyException semesterCalendarEntryAttachmentDependencyException)
            {
                return Problem(semesterCalendarEntryAttachmentDependencyException.Message);
            }
            catch (CalendarEntryAttachmentServiceException semesterCalendarEntryAttachmentServiceException)
            {
                return Problem(semesterCalendarEntryAttachmentServiceException.Message);
            }
        }

        [HttpDelete("calendarentries/{calendarEntryId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<bool>> DeleteCalendarEntryAttachmentAsync(Guid calendarEntryId, Guid attachmentId)
        {
            try
            {
                CalendarEntryAttachment deletedCalendarEntryAttachment =
                    await this.calendarEntryAttachmentService.RemoveCalendarEntryAttachmentByIdAsync(calendarEntryId, attachmentId);

                return Ok(deletedCalendarEntryAttachment);
            }
            catch (CalendarEntryAttachmentValidationException calendarEntryAttachmentValidationException)
                when (calendarEntryAttachmentValidationException.InnerException is NotFoundCalendarEntryAttachmentException)
            {
                return NotFound(calendarEntryAttachmentValidationException.GetInnerMessage());
            }
            catch (CalendarEntryAttachmentValidationException calendarEntryAttachmentValidationException)
            {
                return BadRequest(calendarEntryAttachmentValidationException.GetInnerMessage());
            }
            catch (CalendarEntryAttachmentDependencyException calendarEntryAttachmentValidationException)
               when (calendarEntryAttachmentValidationException.InnerException is LockedCalendarEntryAttachmentException)
            {
                return Locked(calendarEntryAttachmentValidationException.GetInnerMessage());
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

    }
}
