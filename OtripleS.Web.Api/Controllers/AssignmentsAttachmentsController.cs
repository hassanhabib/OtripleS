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

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
