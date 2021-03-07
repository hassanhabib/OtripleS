// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;
using OtripleS.Web.Api.Services.AssignmentAttachments;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentsAttachmentsController : RESTFulController
    {
        private readonly IAssignmentAttachmentService assignmentAttachmentService;

        public AssignmentsAttachmentsController(IAssignmentAttachmentService assignmentAttachmentService) =>
            this.assignmentAttachmentService = assignmentAttachmentService;

        [HttpPost]
        public async ValueTask<ActionResult<AssignmentAttachment>> PostAssignmentAttachmentAsync(
            AssignmentAttachment assignmentAttachment)
        {
            try
            {
                AssignmentAttachment persistedAssignmentAttachment =
                    await this.assignmentAttachmentService.AddAssignmentAttachmentAsync(assignmentAttachment);

                return Ok(persistedAssignmentAttachment);
            }
            catch (AssignmentAttachmentValidationException assignmentAttachmentValidationException)
                when (assignmentAttachmentValidationException.InnerException is AlreadyExistsAssignmentAttachmentException)
            {
                string innerMessage = GetInnerMessage(assignmentAttachmentValidationException);

                return Conflict(innerMessage);
            }
            catch (AssignmentAttachmentValidationException assignmentAttachmentValidationException)
                when (assignmentAttachmentValidationException.InnerException is InvalidAssignmentAttachmentReferenceException)
            {
                string innerMessage = GetInnerMessage(assignmentAttachmentValidationException);

                return FailedDependency(innerMessage);
            }
            catch (AssignmentAttachmentValidationException assignmentAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(assignmentAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (AssignmentAttachmentDependencyException assignmentAttachmentDependencyException)
            {
                return Problem(assignmentAttachmentDependencyException.Message);
            }
            catch (AssignmentAttachmentServiceException assignmentAttachmentServiceException)
            {
                return Problem(assignmentAttachmentServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<AssignmentAttachment>> GetAllAssignmentAttachments()
        {
            try
            {
                IQueryable storageAssignmentAttachments =
                    this.assignmentAttachmentService.RetrieveAllAssignmentAttachments();

                return Ok(storageAssignmentAttachments);
            }
            catch (AssignmentAttachmentDependencyException assignmentAttachmentDependencyException)
            {
                return Problem(assignmentAttachmentDependencyException.Message);
            }
            catch (AssignmentAttachmentServiceException assignmentAttachmentServiceException)
            {
                return Problem(assignmentAttachmentServiceException.Message);
            }
        }

        [HttpGet("assignments/{assignmentId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<AssignmentAttachment>> GetAssignmentAttachmentAsync(
            Guid assignmentId,
            Guid attachmentId)
        {
            try
            {
                AssignmentAttachment storageAssignmentAttachment =
                    await this.assignmentAttachmentService.RetrieveAssignmentAttachmentByIdAsync(assignmentId, attachmentId);

                return Ok(storageAssignmentAttachment);
            }
            catch (AssignmentAttachmentValidationException semesterAssignmentAttachmentValidationException)
                when (semesterAssignmentAttachmentValidationException.InnerException is NotFoundAssignmentAttachmentException)
            {
                string innerMessage = GetInnerMessage(semesterAssignmentAttachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (AssignmentAttachmentValidationException semesterAssignmentAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(semesterAssignmentAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (AssignmentAttachmentDependencyException semesterAssignmentAttachmentDependencyException)
            {
                return Problem(semesterAssignmentAttachmentDependencyException.Message);
            }
            catch (AssignmentAttachmentServiceException semesterAssignmentAttachmentServiceException)
            {
                return Problem(semesterAssignmentAttachmentServiceException.Message);
            }
        }

        [HttpDelete("assignments/{assignmentId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<bool>> DeleteAssignmentAttachmentAsync(
            Guid assignmentId,
            Guid attachmentId)
        {
            try
            {
                AssignmentAttachment deletedAssignmentAttachment =
                    await this.assignmentAttachmentService.RemoveAssignmentAttachmentByIdAsync(
                        assignmentId,
                        attachmentId);

                return Ok(deletedAssignmentAttachment);
            }
            catch (AssignmentAttachmentValidationException assignmentAttachmentValidationException)
                when (assignmentAttachmentValidationException.InnerException is NotFoundAssignmentAttachmentException)
            {
                string innerMessage = GetInnerMessage(assignmentAttachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (AssignmentAttachmentValidationException assignmentAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(assignmentAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (AssignmentAttachmentDependencyException assignmentAttachmentValidationException)
               when (assignmentAttachmentValidationException.InnerException is LockedAssignmentAttachmentException)
            {
                string innerMessage = GetInnerMessage(assignmentAttachmentValidationException);

                return Locked(innerMessage);
            }
            catch (AssignmentAttachmentDependencyException assignmentAttachmentDependencyException)
            {
                return Problem(assignmentAttachmentDependencyException.Message);
            }
            catch (AssignmentAttachmentServiceException assignmentAttachmentServiceException)
            {
                return Problem(assignmentAttachmentServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
