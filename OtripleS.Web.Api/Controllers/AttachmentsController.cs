// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
                Attachment persistedAttachment =
                    await this.attachmentService.AddAttachmentAsync(attachment);

                return Created(persistedAttachment);
            }
            catch (AttachmentValidationException attachmentValidationException)
                when (attachmentValidationException.InnerException is AlreadyExistsAttachmentException)
            {
                string innerMessage = GetInnerMessage(attachmentValidationException);

                return Conflict(innerMessage);
            }
            catch (AttachmentValidationException attachmentValidationException)
            {
                string innerMessage = GetInnerMessage(attachmentValidationException);

                return BadRequest(innerMessage);
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
                string innerMessage = GetInnerMessage(attachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (AttachmentValidationException attachmentValidationException)
            {
                string innerMessage = GetInnerMessage(attachmentValidationException);

                return BadRequest(innerMessage);
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
                string innerMessage = GetInnerMessage(attachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (AttachmentValidationException attachmentValidationException)
            {
                string innerMessage = GetInnerMessage(attachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (AttachmentDependencyException attachmentDependencyException)
                when (attachmentDependencyException.InnerException is LockedAttachmentException)
            {
                string innerMessage = GetInnerMessage(attachmentDependencyException);

                return Locked(innerMessage);
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
                string innerMessage = GetInnerMessage(attachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (AttachmentValidationException attachmentValidationException)
            {
                return BadRequest(attachmentValidationException.Message);
            }
            catch (AttachmentDependencyException attachmentDependencyException)
                when (attachmentDependencyException.InnerException is LockedAttachmentException)
            {
                string innerMessage = GetInnerMessage(attachmentDependencyException);

                return Locked(innerMessage);
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

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}