// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;
using OtripleS.Web.Api.Services.ExamAttachments;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamsAttachmentsController : RESTFulController
    {
        private readonly IExamAttachmentService examAttachmentService;

        public ExamsAttachmentsController(IExamAttachmentService examAttachmentService) =>
            this.examAttachmentService = examAttachmentService;

        [HttpPost]
        public async ValueTask<ActionResult<ExamAttachment>> PostExamAttachmentAsync(
            ExamAttachment examAttachment)
        {
            try
            {
                ExamAttachment persistedExamAttachment =
                    await this.examAttachmentService.AddExamAttachmentAsync(examAttachment);

                return Ok(persistedExamAttachment);
            }
            catch (ExamAttachmentValidationException examAttachmentValidationException)
                when (examAttachmentValidationException.InnerException is AlreadyExistsExamAttachmentException)
            {
                string innerMessage = GetInnerMessage(examAttachmentValidationException);

                return Conflict(innerMessage);
            }
            catch (ExamAttachmentValidationException examAttachmentValidationException)
                when (examAttachmentValidationException.InnerException is InvalidExamAttachmentReferenceException)
            {
                string innerMessage = GetInnerMessage(examAttachmentValidationException);

                return FailedDependency(innerMessage);
            }
            catch (ExamAttachmentValidationException examAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(examAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (ExamAttachmentDependencyException examAttachmentDependencyException)
            {
                return Problem(examAttachmentDependencyException.Message);
            }
            catch (ExamAttachmentServiceException examAttachmentServiceException)
            {
                return Problem(examAttachmentServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<ExamAttachment>> GetAllExamAttachments()
        {
            try
            {
                IQueryable storageExamAttachments =
                    this.examAttachmentService.RetrieveAllExamAttachments();

                return Ok(storageExamAttachments);
            }
            catch (ExamAttachmentDependencyException examAttachmentDependencyException)
            {
                return Problem(examAttachmentDependencyException.Message);
            }
            catch (ExamAttachmentServiceException examAttachmentServiceException)
            {
                return Problem(examAttachmentServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
