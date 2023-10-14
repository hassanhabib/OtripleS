// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Models.GuardianAttachments.Exceptions;
using OtripleS.Web.Api.Services.Foundations.GuardianAttachments;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                GuardianAttachment addedGuardianAttachment =
                    await this.guardianAttachmentService.AddGuardianAttachmentAsync(guardianAttachment);

                return Created(addedGuardianAttachment);
            }
            catch (GuardianAttachmentValidationException guardianAttachmentValidationException)
                when (guardianAttachmentValidationException.InnerException is AlreadyExistsGuardianAttachmentException)
            {
                return Conflict(guardianAttachmentValidationException.GetInnerMessage());
            }
            catch (GuardianAttachmentValidationException guardianAttachmentValidationException)
            {
                return BadRequest(guardianAttachmentValidationException.GetInnerMessage());
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
                return NotFound(semesterGuardianAttachmentValidationException.GetInnerMessage());
            }
            catch (GuardianAttachmentValidationException semesterGuardianAttachmentValidationException)
            {
                return BadRequest(semesterGuardianAttachmentValidationException.GetInnerMessage());
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
                return NotFound(guardianAttachmentValidationException.GetInnerMessage());
            }
            catch (GuardianAttachmentValidationException guardianAttachmentValidationException)
            {
                return BadRequest(guardianAttachmentValidationException.GetInnerMessage());
            }
            catch (GuardianAttachmentDependencyException guardianAttachmentValidationException)
               when (guardianAttachmentValidationException.InnerException is LockedGuardianAttachmentException)
            {
                return Locked(guardianAttachmentValidationException.GetInnerMessage());
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

    }
}
