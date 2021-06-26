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
            catch (ExamAttachmentValidationException semesterExamAttachmentValidationException)
                when (semesterExamAttachmentValidationException.InnerException is NotFoundExamAttachmentException)
            {
                string innerMessage = GetInnerMessage(semesterExamAttachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (ExamAttachmentValidationException semesterExamAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(semesterExamAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (ExamAttachmentDependencyException semesterExamAttachmentDependencyException)
            {
                return Problem(semesterExamAttachmentDependencyException.Message);
            }
            catch (ExamAttachmentServiceException semesterExamAttachmentServiceException)
            {
                return Problem(semesterExamAttachmentServiceException.Message);
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
                string innerMessage = GetInnerMessage(examAttachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (ExamAttachmentValidationException examAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(examAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (ExamAttachmentDependencyException examAttachmentValidationException)
               when (examAttachmentValidationException.InnerException is LockedExamAttachmentException)
            {
                string innerMessage = GetInnerMessage(examAttachmentValidationException);

                return Locked(innerMessage);
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
