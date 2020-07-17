// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;
using OtripleS.Web.Api.Services.Courses;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CoursesController : RESTFulController
	{
		private readonly ICourseService courseService;

		public CoursesController(ICourseService courseService) =>
			this.courseService = courseService;

        [HttpDelete("{courseId}")]
        public async ValueTask<ActionResult<Course>> DeleteCourseAsync(Guid courseId)
        {
            try
            {
                Course storageCourse =
                    await this.courseService.DeleteCourseAsync(courseId);

                return Ok(storageCourse);
            }
            catch (CourseValidationException courseValidationException)
                when (courseValidationException.InnerException is NotFoundCourseException)
            {
                string innerMessage = GetInnerMessage(courseValidationException);

                return NotFound(innerMessage);
            }
            catch (CourseValidationException courseValidationException)
            {
                string innerMessage = GetInnerMessage(courseValidationException);

                return BadRequest(courseValidationException);
            }
            catch (CourseDependencyException courseDependencyException)
               when (courseDependencyException.InnerException is LockedCourseException)
            {
                string innerMessage = GetInnerMessage(courseDependencyException);

                return Locked(innerMessage);
            }
            catch (CourseDependencyException courseDependencyException)
            {
                return Problem(courseDependencyException.Message);
            }
            catch (CourseServiceException courseServiceException)
            {
                return Problem(courseServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
