// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Courses;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : RESTFulController
    {
        private readonly ICourseService courseService;

        public CoursesController(ICourseService courseService) =>
            this.courseService = courseService;

        [HttpPost]
        public async ValueTask<ActionResult<Course>> PostCourseAsync(Course course)
        {
            try
            {
                Course createdCourse =
                    await this.courseService.CreateCourseAsync(course);

                return Created(createdCourse);
            }
            catch (CourseValidationException courseValidationException)
                when (courseValidationException.InnerException is AlreadyExistsCourseException)
            {
                return Conflict(courseValidationException.InnerException);
            }
            catch (CourseValidationException courseValidationException)
            {
                return BadRequest(courseValidationException.InnerException);
            }
            catch (CourseDependencyException courseDependencyException)
            {
                return InternalServerError(courseDependencyException);
            }
            catch (CourseServiceException courseServiceException)
            {
                return InternalServerError(courseServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Course>> GetAllCourses()
        {
            try
            {
                IQueryable storageCourse =
                    this.courseService.RetrieveAllCourses();

                return Ok(storageCourse);
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

        [HttpGet("{courseId}")]
        public async ValueTask<ActionResult<Course>> GetCourseAsync(Guid courseId)
        {
            try
            {
                Course storageCourse =
                    await this.courseService.RetrieveCourseByIdAsync(courseId);

                return Ok(storageCourse);
            }
            catch (CourseValidationException courseValidationException)
                when (courseValidationException.InnerException is NotFoundCourseException)
            {
                return NotFound(courseValidationException.GetInnerMessage());
            }
            catch (CourseValidationException courseValidationException)
            {
                return BadRequest(courseValidationException.GetInnerMessage());
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

        [HttpPut]
        public async ValueTask<ActionResult<Course>> PutCourseAsync(Course course)
        {
            try
            {
                Course registeredCourse =
                    await this.courseService.ModifyCourseAsync(course);

                return Ok(registeredCourse);
            }
            catch (CourseValidationException courseValidationException)
                when (courseValidationException.InnerException is NotFoundCourseException)
            {
                return NotFound(courseValidationException.GetInnerMessage());
            }
            catch (CourseValidationException courseValidationException)
            {
                return BadRequest(courseValidationException.GetInnerMessage());
            }
            catch (CourseDependencyException courseDependencyException)
                when (courseDependencyException.InnerException is LockedCourseException)
            {
                return Locked(courseDependencyException.GetInnerMessage());
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

        [HttpDelete("{courseId}")]
        public async ValueTask<ActionResult<Course>> DeleteCourseAsync(Guid courseId)
        {
            try
            {
                Course storageCourse =
                    await this.courseService.RemoveCourseAsync(courseId);

                return Ok(storageCourse);
            }
            catch (CourseValidationException courseValidationException)
                when (courseValidationException.InnerException is NotFoundCourseException)
            {
                return NotFound(courseValidationException.GetInnerMessage());
            }
            catch (CourseValidationException courseValidationException)
            {
                return BadRequest(courseValidationException.GetInnerMessage());
            }
            catch (CourseDependencyException courseDependencyException)
               when (courseDependencyException.InnerException is LockedCourseException)
            {
                return Locked(courseDependencyException.GetInnerMessage());
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

    }
}