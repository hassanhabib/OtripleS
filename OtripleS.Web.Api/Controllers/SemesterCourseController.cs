// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using OtripleS.Web.Api.Services.SemesterCourses;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterCourseController : RESTFulController
    {
        private readonly ISemesterCourseService semesterCourseService;

        public SemesterCourseController(ISemesterCourseService semesterCourseService) =>
            this.semesterCourseService = semesterCourseService;

        [HttpPost]
        public async ValueTask<ActionResult<SemesterCourse>> PostSemesterCourseAsync(
            SemesterCourse semesterCourse)
        {
            try
            {
                SemesterCourse persistedSemesterCourse =
                    await this.semesterCourseService.CreateSemesterCourseAsync(semesterCourse);

                return Ok(persistedSemesterCourse);
            }
            catch (SemesterCourseValidationException semesterCourseValidationException)
                when (semesterCourseValidationException.InnerException is AlreadyExistsSemesterCourseException)
            {
                string innerMessage = GetInnerMessage(semesterCourseValidationException);

                return Conflict(innerMessage);
            }
            catch (SemesterCourseValidationException semesterCourseValidationException)
            {
                string innerMessage = GetInnerMessage(semesterCourseValidationException);

                return BadRequest(innerMessage);
            }
            catch (SemesterCourseDependencyException semesterCourseDependencyException)
            {
                return Problem(semesterCourseDependencyException.Message);
            }
            catch (SemesterCourseServiceException semesterCourseServiceException)
            {
                return Problem(semesterCourseServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<SemesterCourse>> PutSemesterCourseAsync(SemesterCourse semesterCourse)
        {
            try
            {
                SemesterCourse registeredSemesterCourse =
                    await this.semesterCourseService.ModifySemesterCourseAsync(semesterCourse);

                return Ok(registeredSemesterCourse);
            }
            catch (SemesterCourseValidationException semesterCourseValidationException)
                when (semesterCourseValidationException.InnerException is NotFoundSemesterCourseException)
            {
                string innerMessage = GetInnerMessage(semesterCourseValidationException);

                return NotFound(innerMessage);
            }
            catch (SemesterCourseValidationException semesterCourseValidationException)
            {
                string innerMessage = GetInnerMessage(semesterCourseValidationException);

                return BadRequest(innerMessage);
            }
            catch (SemesterCourseDependencyException semesterCourseDependencyException)
                when (semesterCourseDependencyException.InnerException is LockedSemesterCourseException)
            {
                string innerMessage = GetInnerMessage(semesterCourseDependencyException);

                return Locked(innerMessage);
            }
            catch (SemesterCourseDependencyException semesterCourseDependencyException)
            {
                return Problem(semesterCourseDependencyException.Message);
            }
            catch (SemesterCourseServiceException semesterCourseServiceException)
            {
                return Problem(semesterCourseServiceException.Message);
            }
        }

        [HttpDelete("{semesterCourseId}")]
        public async ValueTask<ActionResult<SemesterCourse>> DeleteSemesterCourseAsync(Guid semesterCourseId)
        {
            try
            {
                SemesterCourse storageSemesterCourse =
                    await this.semesterCourseService.DeleteSemesterCourseAsync(semesterCourseId);

                return Ok(storageSemesterCourse);
            }
            catch (SemesterCourseValidationException semesterCourseValidationException)
                when (semesterCourseValidationException.InnerException is NotFoundSemesterCourseException)
            {
                string innerMessage = GetInnerMessage(semesterCourseValidationException);

                return NotFound(innerMessage);
            }
            catch (SemesterCourseValidationException semesterCourseValidationException)
            {
                return BadRequest(semesterCourseValidationException.Message);
            }
            catch (SemesterCourseDependencyException semesterCourseDependencyException)
                when (semesterCourseDependencyException.InnerException is LockedSemesterCourseException)
            {
                string innerMessage = GetInnerMessage(semesterCourseDependencyException);

                return Locked(innerMessage);
            }
            catch (SemesterCourseDependencyException semesterCourseDependencyException)
            {
                return Problem(semesterCourseDependencyException.Message);
            }
            catch (SemesterCourseServiceException semesterCourseServiceException)
            {
                return Problem(semesterCourseServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
