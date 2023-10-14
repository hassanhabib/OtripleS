// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Attachments.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Attachments;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentsController : RESTFulController
    {
        private readonly IAttachmentService attachmentService;

        public AttachmentsController(IAttachmentService attachmentService) =>
            this.attachmentService = attachmentService;

        [HttpPost]
        public async ValueTask<ActionResult<Attachment>> PostAttachmentAsync(Attachment attachment)
        {
            try
            {
                Attachment addedAttachment =
                    await this.attachmentService.AddAttachmentAsync(attachment);

                return Created(addedAttachment);
            }
            catch (AttachmentValidationException attachmentValidationException)
                when (attachmentValidationException.InnerException is AlreadyExistsAttachmentException)
            {
                return Conflict(attachmentValidationException.GetInnerMessage());
            }
            catch (AttachmentValidationException attachmentValidationException)
            {
                return BadRequest(attachmentValidationException.GetInnerMessage());
            }
            catch (AttachmentDependencyException attachmentDependencyException)
            {
                return Problem(attachmentDependencyException.Message);
            }
            catch (AttachmentServiceException attachmentServiceException)
            {
                return Problem(attachmentServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Attachment>> GetAllAttachments()
        {
            try
            {
                IQueryable storageAttachments =
                    this.attachmentService.RetrieveAllAttachments();

                return Ok(storageAttachments);
            }
            catch (AttachmentDependencyException attachmentDependencyException)
            {
                return Problem(attachmentDependencyException.Message);
            }
            catch (AttachmentServiceException attachmentServiceException)
            {
                return Problem(attachmentServiceException.Message);
            }
        }

        [HttpGet("{attachmentId}")]
        public async ValueTask<ActionResult<Attachment>> GetAttachmentByIdAsync(Guid attachmentId)
        {
            try
            {
                Attachment storageAttachment =
                    await this.attachmentService.RetrieveAttachmentByIdAsync(attachmentId);

                return Ok(storageAttachment);
            }
            catch (AttachmentValidationException attachmentValidationException)
                when (attachmentValidationException.InnerException is NotFoundAttachmentException)
            {
                return NotFound(attachmentValidationException.GetInnerMessage());
            }
            catch (AttachmentValidationException attachmentValidationException)
            {
                return BadRequest(attachmentValidationException.GetInnerMessage());
            }
            catch (AttachmentDependencyException attachmentDependencyException)
            {
                return Problem(attachmentDependencyException.Message);
            }
            catch (AttachmentServiceException attachmentServiceException)
            {
                return Problem(attachmentServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Attachment>> PutAttachmentAsync(Attachment attachment)
        {
            try
            {
                Attachment registeredAttachment =
                    await this.attachmentService.ModifyAttachmentAsync(attachment);

                return Ok(registeredAttachment);
            }
            catch (AttachmentValidationException attachmentValidationException)
                when (attachmentValidationException.InnerException is NotFoundAttachmentException)
            {
                return NotFound(attachmentValidationException.GetInnerMessage());
            }
            catch (AttachmentValidationException attachmentValidationException)
            {
                return BadRequest(attachmentValidationException.GetInnerMessage());
            }
            catch (AttachmentDependencyException attachmentDependencyException)
                when (attachmentDependencyException.InnerException is LockedAttachmentException)
            {
                return Locked(attachmentDependencyException.GetInnerMessage());
            }
            catch (AttachmentDependencyException attachmentDependencyException)
            {
                return Problem(attachmentDependencyException.Message);
            }
            catch (AttachmentServiceException attachmentServiceException)
            {
                return Problem(attachmentServiceException.Message);
            }
        }

        [HttpDelete("{attachmentId}")]
        public async ValueTask<ActionResult<Attachment>> DeleteAttachmentAsync(Guid attachmentId)
        {
            try
            {
                Attachment storageAttachment =
                    await this.attachmentService.RemoveAttachmentByIdAsync(attachmentId);

                return Ok(storageAttachment);
            }
            catch (AttachmentValidationException attachmentValidationException)
                when (attachmentValidationException.InnerException is NotFoundAttachmentException)
            {
                return NotFound(attachmentValidationException.GetInnerMessage());
            }
            catch (AttachmentValidationException attachmentValidationException)
            {
                return BadRequest(attachmentValidationException.Message);
            }
            catch (AttachmentDependencyException attachmentDependencyException)
                when (attachmentDependencyException.InnerException is LockedAttachmentException)
            {
                return Locked(attachmentDependencyException.GetInnerMessage());
            }
            catch (AttachmentDependencyException attachmentDependencyException)
            {
                return Problem(attachmentDependencyException.Message);
            }
            catch (AttachmentServiceException attachmentServiceException)
            {
                return Problem(attachmentServiceException.Message);
            }
        }

    }
}