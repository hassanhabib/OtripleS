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
using OtripleS.Web.Api.Services.Foundations.ExamAttachments;
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

                return Created(persistedExamAttachment);
            }
            catch (ExamAttachmentValidationException examAttachmentValidationException)
                when (examAttachmentValidationException.InnerException is AlreadyExistsExamAttachmentException)
            {
                return Conflict(examAttachmentValidationException.InnerException);
            }
            catch (ExamAttachmentValidationException examAttachmentValidationException)
                when (examAttachmentValidationException.InnerException is InvalidExamAttachmentReferenceException)
            {
                return FailedDependency(examAttachmentValidationException.InnerException);
            }
            catch (ExamAttachmentValidationException examAttachmentValidationException)
            {
                return BadRequest(examAttachmentValidationException.InnerException);
            }
            catch (ExamAttachmentDependencyException examAttachmentDependencyException)
            {
                return InternalServerError(examAttachmentDependencyException);
            }
            catch (ExamAttachmentServiceException examAttachmentServiceException)
            {
                return InternalServerError(examAttachmentServiceException);
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
                return InternalServerError(examAttachmentDependencyException);
            }
            catch (ExamAttachmentServiceException examAttachmentServiceException)
            {
                return InternalServerError(examAttachmentServiceException);
            }
        }

        [HttpGet("exams/{examId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<ExamAttachment>> GetExamAttachmentByIdsAsync(
            Guid examId,
            Guid attachmentId)
        {
            try
            {
                ExamAttachment storageExamAttachment =
                    await this.examAttachmentService.RetrieveExamAttachmentByIdAsync(examId, attachmentId);

                return Ok(storageExamAttachment);
            }
            catch (ExamAttachmentValidationException examAttachmentValidationException)
                when (examAttachmentValidationException.InnerException is NotFoundExamAttachmentException)
            {
                return NotFound(examAttachmentValidationException.InnerException);
            }
            catch (ExamAttachmentValidationException examAttachmentValidationException)
            {
                return BadRequest(examAttachmentValidationException.InnerException);
            }
            catch (ExamAttachmentDependencyException examAttachmentDependencyException)
            {
                return InternalServerError(examAttachmentDependencyException);
            }
            catch (ExamAttachmentServiceException examAttachmentServiceException)
            {
                return InternalServerError(examAttachmentServiceException);
            }
        }

        [HttpDelete("exams/{examId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<bool>> DeleteExamAttachmentByIdsAsync(Guid examId, Guid attachmentId)
        {
            try
            {
                ExamAttachment deletedExamAttachment =
                    await this.examAttachmentService.RemoveExamAttachmentByIdAsync(examId, attachmentId);

                return Ok(deletedExamAttachment);
            }
            catch (ExamAttachmentValidationException examAttachmentValidationException)
                when (examAttachmentValidationException.InnerException is NotFoundExamAttachmentException)
            {
                return NotFound(examAttachmentValidationException.InnerException);
            }
            catch (ExamAttachmentValidationException examAttachmentValidationException)
            {
                return BadRequest(examAttachmentValidationException.InnerException);
            }
            catch (ExamAttachmentDependencyException examAttachmentDependencyException)
               when (examAttachmentDependencyException.InnerException is LockedExamAttachmentException)
            {
                return Locked(examAttachmentDependencyException.InnerException);
            }
            catch (ExamAttachmentDependencyException examAttachmentDependencyException)
            {
                return InternalServerError(examAttachmentDependencyException);
            }
            catch (ExamAttachmentServiceException examAttachmentServiceException)
            {
                return InternalServerError(examAttachmentServiceException);
            }
        }
    }
}
