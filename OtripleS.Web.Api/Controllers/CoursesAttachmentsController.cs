// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;
using OtripleS.Web.Api.Services.Foundations.CourseAttachments;
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
                CourseAttachment addedCourseAttachment =
                    await this.courseAttachmentService.AddCourseAttachmentAsync(courseAttachment);

                return Created(addedCourseAttachment);
            }
            catch (CourseAttachmentValidationException courseAttachmentValidationException)
                when (courseAttachmentValidationException.InnerException is AlreadyExistsCourseAttachmentException)
            {
                return Conflict(courseAttachmentValidationException.GetInnerMessage());
            }
            catch (CourseAttachmentValidationException courseAttachmentValidationException)
                when (courseAttachmentValidationException.InnerException is InvalidCourseAttachmentReferenceException)
            {
                return FailedDependency(courseAttachmentValidationException.GetInnerMessage());
            }
            catch (CourseAttachmentValidationException courseAttachmentValidationException)
            {
                return BadRequest(courseAttachmentValidationException.GetInnerMessage());
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
                return NotFound(semesterCourseAttachmentValidationException.GetInnerMessage());
            }
            catch (CourseAttachmentValidationException semesterCourseAttachmentValidationException)
            {
                return BadRequest(semesterCourseAttachmentValidationException.GetInnerMessage());
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
                return NotFound(courseAttachmentValidationException.GetInnerMessage());
            }
            catch (CourseAttachmentValidationException courseAttachmentValidationException)
            {
                return BadRequest(courseAttachmentValidationException.GetInnerMessage());
            }
            catch (CourseAttachmentDependencyException courseAttachmentValidationException)
               when (courseAttachmentValidationException.InnerException is LockedCourseAttachmentException)
            {
                return Locked(courseAttachmentValidationException.GetInnerMessage());
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

    }
}
