// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.StudentAttachments;
using OtripleS.Web.Api.Models.StudentAttachments.Exceptions;
using OtripleS.Web.Api.Services.Foundations.StudentAttachments;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentAttachmentsController : RESTFulController
    {
        private readonly IStudentAttachmentService studentAttachmentService;

        public StudentAttachmentsController(IStudentAttachmentService studentAttachmentService) =>
            this.studentAttachmentService = studentAttachmentService;

        [HttpPost]
        public async ValueTask<ActionResult<StudentAttachment>> PostStudentAttachmentAsync(
            StudentAttachment studentAttachment)
        {
            try
            {
                StudentAttachment addedStudentAttachment =
                    await this.studentAttachmentService.AddStudentAttachmentAsync(studentAttachment);

                return Created(addedStudentAttachment);
            }
            catch (StudentAttachmentValidationException studentAttachmentValidationException)
                when (studentAttachmentValidationException.InnerException is AlreadyExistsStudentAttachmentException)
            {
                return Conflict(studentAttachmentValidationException.GetInnerMessage());
            }
            catch (StudentAttachmentValidationException studentAttachmentValidationException)
            {
                return BadRequest(studentAttachmentValidationException.GetInnerMessage());
            }
            catch (StudentAttachmentDependencyException studentAttachmentDependencyException)
            {
                return Problem(studentAttachmentDependencyException.Message);
            }
            catch (StudentAttachmentServiceException studentAttachmentServiceException)
            {
                return Problem(studentAttachmentServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<StudentAttachment>> GetAllStudentAttachments()
        {
            try
            {
                IQueryable storageStudentAttachments =
                    this.studentAttachmentService.RetrieveAllStudentAttachments();

                return Ok(storageStudentAttachments);
            }
            catch (StudentAttachmentDependencyException studentAttachmentDependencyException)
            {
                return Problem(studentAttachmentDependencyException.Message);
            }
            catch (StudentAttachmentServiceException studentAttachmentServiceException)
            {
                return Problem(studentAttachmentServiceException.Message);
            }
        }

        [HttpGet("students/{studentId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<StudentAttachment>> GetStudentAttachmentAsync(
            Guid studentId,
            Guid attachmentId)
        {
            try
            {
                StudentAttachment storageStudentAttachment =
                    await this.studentAttachmentService.RetrieveStudentAttachmentByIdAsync(studentId, attachmentId);

                return Ok(storageStudentAttachment);
            }
            catch (StudentAttachmentValidationException semesterStudentAttachmentValidationException)
                when (semesterStudentAttachmentValidationException.InnerException is NotFoundStudentAttachmentException)
            {
                return NotFound(semesterStudentAttachmentValidationException.GetInnerMessage());
            }
            catch (StudentAttachmentValidationException semesterStudentAttachmentValidationException)
            {
                return BadRequest(semesterStudentAttachmentValidationException.GetInnerMessage());
            }
            catch (StudentAttachmentDependencyException semesterStudentAttachmentDependencyException)
            {
                return Problem(semesterStudentAttachmentDependencyException.Message);
            }
            catch (StudentAttachmentServiceException semesterStudentAttachmentServiceException)
            {
                return Problem(semesterStudentAttachmentServiceException.Message);
            }
        }

        [HttpDelete("students/{studentId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<bool>> DeleteStudentAttachmentAsync(Guid studentId, Guid attachmentId)
        {
            try
            {
                StudentAttachment deletedStudentAttachment =
                    await this.studentAttachmentService.RemoveStudentAttachmentByIdAsync(studentId, attachmentId);

                return Ok(deletedStudentAttachment);
            }
            catch (StudentAttachmentValidationException studentAttachmentValidationException)
                when (studentAttachmentValidationException.InnerException is NotFoundStudentAttachmentException)
            {
                return NotFound(studentAttachmentValidationException.GetInnerMessage());
            }
            catch (StudentAttachmentValidationException studentAttachmentValidationException)
            {
                return BadRequest(studentAttachmentValidationException.GetInnerMessage());
            }
            catch (StudentAttachmentDependencyException studentAttachmentValidationException)
               when (studentAttachmentValidationException.InnerException is LockedStudentAttachmentException)
            {
                return Locked(studentAttachmentValidationException.GetInnerMessage());
            }
            catch (StudentAttachmentDependencyException studentAttachmentDependencyException)
            {
                return Problem(studentAttachmentDependencyException.Message);
            }
            catch (StudentAttachmentServiceException studentAttachmentServiceException)
            {
                return Problem(studentAttachmentServiceException.Message);
            }
        }

    }
}
