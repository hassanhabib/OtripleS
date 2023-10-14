// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.TeacherAttachments;
using OtripleS.Web.Api.Models.TeacherAttachments.Exceptions;
using OtripleS.Web.Api.Services.Foundations.TeacherAttachments;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherAttachmentsController : RESTFulController
    {
        private readonly ITeacherAttachmentService teacherAttachmentService;

        public TeacherAttachmentsController(ITeacherAttachmentService teacherAttachmentService) =>
            this.teacherAttachmentService = teacherAttachmentService;

        [HttpPost]
        public async ValueTask<ActionResult<TeacherAttachment>> PostTeacherAttachmentAsync(
            TeacherAttachment teacherAttachment)
        {
            try
            {
                TeacherAttachment addedTeacherAttachment =
                    await this.teacherAttachmentService.AddTeacherAttachmentAsync(teacherAttachment);

                return Created(addedTeacherAttachment);
            }
            catch (TeacherAttachmentValidationException teacherAttachmentValidationException)
                when (teacherAttachmentValidationException.InnerException is AlreadyExistsTeacherAttachmentException)
            {
                return Conflict(teacherAttachmentValidationException.GetInnerMessage());
            }
            catch (TeacherAttachmentValidationException teacherAttachmentValidationException)
                when (teacherAttachmentValidationException.InnerException is InvalidTeacherAttachmentReferenceException)
            {
                return FailedDependency(teacherAttachmentValidationException.GetInnerMessage());
            }
            catch (TeacherAttachmentValidationException teacherAttachmentValidationException)
            {
                return BadRequest(teacherAttachmentValidationException.GetInnerMessage());
            }
            catch (TeacherAttachmentDependencyException teacherAttachmentDependencyException)
            {
                return Problem(teacherAttachmentDependencyException.Message);
            }
            catch (TeacherAttachmentServiceException teacherAttachmentServiceException)
            {
                return Problem(teacherAttachmentServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<TeacherAttachment>> GetAllTeacherAttachments()
        {
            try
            {
                IQueryable storageTeacherAttachments =
                    this.teacherAttachmentService.RetrieveAllTeacherAttachments();

                return Ok(storageTeacherAttachments);
            }
            catch (TeacherAttachmentDependencyException teacherAttachmentDependencyException)
            {
                return Problem(teacherAttachmentDependencyException.Message);
            }
            catch (TeacherAttachmentServiceException teacherAttachmentServiceException)
            {
                return Problem(teacherAttachmentServiceException.Message);
            }
        }

        [HttpGet("teachers/{teacherId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<TeacherAttachment>> GetTeacherAttachmentAsync(
            Guid teacherId,
            Guid attachmentId)
        {
            try
            {
                TeacherAttachment storageTeacherAttachment =
                    await this.teacherAttachmentService.RetrieveTeacherAttachmentByIdAsync(teacherId, attachmentId);

                return Ok(storageTeacherAttachment);
            }
            catch (TeacherAttachmentValidationException semesterTeacherAttachmentValidationException)
                when (semesterTeacherAttachmentValidationException.InnerException is NotFoundTeacherAttachmentException)
            {
                return NotFound(semesterTeacherAttachmentValidationException.GetInnerMessage());
            }
            catch (TeacherAttachmentValidationException semesterTeacherAttachmentValidationException)
            {
                return BadRequest(semesterTeacherAttachmentValidationException.GetInnerMessage());
            }
            catch (TeacherAttachmentDependencyException semesterTeacherAttachmentDependencyException)
            {
                return Problem(semesterTeacherAttachmentDependencyException.Message);
            }
            catch (TeacherAttachmentServiceException semesterTeacherAttachmentServiceException)
            {
                return Problem(semesterTeacherAttachmentServiceException.Message);
            }
        }

        [HttpDelete("teachers/{teacherId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<bool>> DeleteTeacherAttachmentAsync(Guid teacherId, Guid attachmentId)
        {
            try
            {
                TeacherAttachment deletedTeacherAttachment =
                    await this.teacherAttachmentService.RemoveTeacherAttachmentByIdAsync(teacherId, attachmentId);

                return Ok(deletedTeacherAttachment);
            }
            catch (TeacherAttachmentValidationException teacherAttachmentValidationException)
                when (teacherAttachmentValidationException.InnerException is NotFoundTeacherAttachmentException)
            {
                return NotFound(teacherAttachmentValidationException.GetInnerMessage());
            }
            catch (TeacherAttachmentValidationException teacherAttachmentValidationException)
            {
                return BadRequest(teacherAttachmentValidationException.GetInnerMessage());
            }
            catch (TeacherAttachmentDependencyException teacherAttachmentValidationException)
               when (teacherAttachmentValidationException.InnerException is LockedTeacherAttachmentException)
            {
                return Locked(teacherAttachmentValidationException.GetInnerMessage());
            }
            catch (TeacherAttachmentDependencyException teacherAttachmentDependencyException)
            {
                return Problem(teacherAttachmentDependencyException.Message);
            }
            catch (TeacherAttachmentServiceException teacherAttachmentServiceException)
            {
                return Problem(teacherAttachmentServiceException.Message);
            }
        }

    }
}
