// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Foundations.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.Foundations.CalendarEntryAttachments.Exceptions;
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
                string innerMessage = GetInnerMessage(semesterCalendarEntryAttachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (CalendarEntryAttachmentValidationException semesterCalendarEntryAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(semesterCalendarEntryAttachmentValidationException);

                return BadRequest(innerMessage);
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
                string innerMessage = GetInnerMessage(calendarEntryAttachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (CalendarEntryAttachmentValidationException calendarEntryAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(calendarEntryAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (CalendarEntryAttachmentDependencyException calendarEntryAttachmentValidationException)
               when (calendarEntryAttachmentValidationException.InnerException is LockedCalendarEntryAttachmentException)
            {
                string innerMessage = GetInnerMessage(calendarEntryAttachmentValidationException);

                return Locked(innerMessage);
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
