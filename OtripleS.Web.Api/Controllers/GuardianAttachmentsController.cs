// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Models.GuardianAttachments.Exceptions;
using OtripleS.Web.Api.Services.Foundations.GuardianAttachments;
using RESTFulSense.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuardianAttachmentsController : RESTFulController
    {
        private readonly IGuardianAttachmentService guardianAttachmentService;

        public GuardianAttachmentsController(IGuardianAttachmentService guardianAttachmentService) =>
            this.guardianAttachmentService = guardianAttachmentService;

        [HttpPost]
        public async ValueTask<ActionResult<GuardianAttachment>> PostGuardianAttachmentAsync(
            GuardianAttachment guardianAttachment)
        {
            try
            {
                GuardianAttachment persistedGuardianAttachment =
                    await this.guardianAttachmentService.AddGuardianAttachmentAsync(guardianAttachment);

                return Created(persistedGuardianAttachment);
            }
            catch (GuardianAttachmentValidationException guardianAttachmentValidationException)
                when (guardianAttachmentValidationException.InnerException is AlreadyExistsGuardianAttachmentException)
            {
                string innerMessage = GetInnerMessage(guardianAttachmentValidationException);

                return Conflict(innerMessage);
            }
            catch (GuardianAttachmentValidationException guardianAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(guardianAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (GuardianAttachmentDependencyException guardianAttachmentDependencyException)
            {
                return Problem(guardianAttachmentDependencyException.Message);
            }
            catch (GuardianAttachmentServiceException guardianAttachmentServiceException)
            {
                return Problem(guardianAttachmentServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<GuardianAttachment>> GetAllGuardianAttachments()
        {
            try
            {
                IQueryable storageGuardianAttachments =
                    this.guardianAttachmentService.RetrieveAllGuardianAttachments();

                return Ok(storageGuardianAttachments);
            }
            catch (GuardianAttachmentDependencyException guardianAttachmentDependencyException)
            {
                return Problem(guardianAttachmentDependencyException.Message);
            }
            catch (GuardianAttachmentServiceException guardianAttachmentServiceException)
            {
                return Problem(guardianAttachmentServiceException.Message);
            }
        }

        [HttpGet("guardians/{guardianId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<GuardianAttachment>> GetGuardianAttachmentAsync(
            Guid guardianId,
            Guid attachmentId)
        {
            try
            {
                GuardianAttachment storageGuardianAttachment =
                    await this.guardianAttachmentService.RetrieveGuardianAttachmentByIdAsync(guardianId, attachmentId);

                return Ok(storageGuardianAttachment);
            }
            catch (GuardianAttachmentValidationException semesterGuardianAttachmentValidationException)
                when (semesterGuardianAttachmentValidationException.InnerException is NotFoundGuardianAttachmentException)
            {
                string innerMessage = GetInnerMessage(semesterGuardianAttachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (GuardianAttachmentValidationException semesterGuardianAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(semesterGuardianAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (GuardianAttachmentDependencyException semesterGuardianAttachmentDependencyException)
            {
                return Problem(semesterGuardianAttachmentDependencyException.Message);
            }
            catch (GuardianAttachmentServiceException semesterGuardianAttachmentServiceException)
            {
                return Problem(semesterGuardianAttachmentServiceException.Message);
            }
        }

        [HttpDelete("guardians/{guardianId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<bool>> DeleteGuardianAttachmentAsync(Guid guardianId, Guid attachmentId)
        {
            try
            {
                GuardianAttachment deletedGuardianAttachment =
                    await this.guardianAttachmentService.RemoveGuardianAttachmentByIdAsync(guardianId, attachmentId);

                return Ok(deletedGuardianAttachment);
            }
            catch (GuardianAttachmentValidationException guardianAttachmentValidationException)
                when (guardianAttachmentValidationException.InnerException is NotFoundGuardianAttachmentException)
            {
                string innerMessage = GetInnerMessage(guardianAttachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (GuardianAttachmentValidationException guardianAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(guardianAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (GuardianAttachmentDependencyException guardianAttachmentValidationException)
               when (guardianAttachmentValidationException.InnerException is LockedGuardianAttachmentException)
            {
                string innerMessage = GetInnerMessage(guardianAttachmentValidationException);

                return Locked(innerMessage);
            }
            catch (GuardianAttachmentDependencyException guardianAttachmentDependencyException)
            {
                return Problem(guardianAttachmentDependencyException.Message);
            }
            catch (GuardianAttachmentServiceException guardianAttachmentServiceException)
            {
                return Problem(guardianAttachmentServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
