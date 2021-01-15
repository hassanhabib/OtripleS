// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Models.GuardianAttachments.Exceptions;
using OtripleS.Web.Api.Services.GuardianAttachments;
using RESTFulSense.Controllers;

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

                return Ok(persistedGuardianAttachment);
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

        

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
