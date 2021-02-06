// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;
using OtripleS.Web.Api.Services.CourseAttachments;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesAttachmentsController : RESTFulController
    {
        private readonly ICourseAttachmentService courseAttachmentService;

        public CoursesAttachmentsController(ICourseAttachmentService courseAttachmentService) =>
            this.courseAttachmentService = courseAttachmentService;

        [HttpPost]
        public async ValueTask<ActionResult<CourseAttachment>> PostCourseAttachmentAsync(
            CourseAttachment courseAttachment)
        {
            try
            {
                CourseAttachment persistedCourseAttachment =
                    await this.courseAttachmentService.AddCourseAttachmentAsync(courseAttachment);

                return Ok(persistedCourseAttachment);
            }
            catch (CourseAttachmentValidationException courseAttachmentValidationException)
                when (courseAttachmentValidationException.InnerException is AlreadyExistsCourseAttachmentException)
            {
                string innerMessage = GetInnerMessage(courseAttachmentValidationException);

                return Conflict(innerMessage);
            }
            catch (CourseAttachmentValidationException courseAttachmentValidationException)
                when (courseAttachmentValidationException.InnerException is InvalidCourseAttachmentReferenceException)
            {
                string innerMessage = GetInnerMessage(courseAttachmentValidationException);

                return FailedDependency(innerMessage);
            }
            catch (CourseAttachmentValidationException courseAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(courseAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (CourseAttachmentDependencyException courseAttachmentDependencyException)
            {
                return Problem(courseAttachmentDependencyException.Message);
            }
            catch (CourseAttachmentServiceException courseAttachmentServiceException)
            {
                return Problem(courseAttachmentServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<CourseAttachment>> GetAllCourseAttachments()
        {
            try
            {
                IQueryable storageCourseAttachments =
                    this.courseAttachmentService.RetrieveAllCourseAttachments();

                return Ok(storageCourseAttachments);
            }
            catch (CourseAttachmentDependencyException courseAttachmentDependencyException)
            {
                return Problem(courseAttachmentDependencyException.Message);
            }
            catch (CourseAttachmentServiceException courseAttachmentServiceException)
            {
                return Problem(courseAttachmentServiceException.Message);
            }
        }

        [HttpGet("courses/{courseId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<CourseAttachment>> GetCourseAttachmentAsync(
            Guid courseId,
            Guid attachmentId)
        {
            try
            {
                CourseAttachment storageCourseAttachment =
                    await this.courseAttachmentService.RetrieveCourseAttachmentByIdAsync(courseId, attachmentId);

                return Ok(storageCourseAttachment);
            }
            catch (CourseAttachmentValidationException semesterCourseAttachmentValidationException)
                when (semesterCourseAttachmentValidationException.InnerException is NotFoundCourseAttachmentException)
            {
                string innerMessage = GetInnerMessage(semesterCourseAttachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (CourseAttachmentValidationException semesterCourseAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(semesterCourseAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (CourseAttachmentDependencyException semesterCourseAttachmentDependencyException)
            {
                return Problem(semesterCourseAttachmentDependencyException.Message);
            }
            catch (CourseAttachmentServiceException semesterCourseAttachmentServiceException)
            {
                return Problem(semesterCourseAttachmentServiceException.Message);
            }
        }

        [HttpDelete("courses/{courseId}/attachments/{attachmentId}")]
        public async ValueTask<ActionResult<bool>> DeleteCourseAttachmentAsync(Guid courseId, Guid attachmentId)
        {
            try
            {
                CourseAttachment deletedCourseAttachment =
                    await this.courseAttachmentService.RemoveCourseAttachmentByIdAsync(courseId, attachmentId);

                return Ok(deletedCourseAttachment);
            }
            catch (CourseAttachmentValidationException courseAttachmentValidationException)
                when (courseAttachmentValidationException.InnerException is NotFoundCourseAttachmentException)
            {
                string innerMessage = GetInnerMessage(courseAttachmentValidationException);

                return NotFound(innerMessage);
            }
            catch (CourseAttachmentValidationException courseAttachmentValidationException)
            {
                string innerMessage = GetInnerMessage(courseAttachmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (CourseAttachmentDependencyException courseAttachmentValidationException)
               when (courseAttachmentValidationException.InnerException is LockedCourseAttachmentException)
            {
                string innerMessage = GetInnerMessage(courseAttachmentValidationException);

                return Locked(innerMessage);
            }
            catch (CourseAttachmentDependencyException courseAttachmentDependencyException)
            {
                return Problem(courseAttachmentDependencyException.Message);
            }
            catch (CourseAttachmentServiceException courseAttachmentServiceException)
            {
                return Problem(courseAttachmentServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
