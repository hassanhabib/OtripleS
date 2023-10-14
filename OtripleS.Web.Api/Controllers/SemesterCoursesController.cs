// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using OtripleS.Web.Api.Services.Foundations.SemesterCourses;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemesterCoursesController : RESTFulController
    {
        private readonly ISemesterCourseService semesterCourseService;

        public SemesterCoursesController(ISemesterCourseService semesterCourseService) =>
            this.semesterCourseService = semesterCourseService;

        [HttpPost]
        public async ValueTask<ActionResult<SemesterCourse>> PostSemesterCourseAsync(
            SemesterCourse semesterCourse)
        {
            try
            {
                SemesterCourse createdSemesterCourse =
                    await this.semesterCourseService.CreateSemesterCourseAsync(semesterCourse);

                return Created(createdSemesterCourse);
            }
            catch (SemesterCourseValidationException semesterCourseValidationException)
                when (semesterCourseValidationException.InnerException is AlreadyExistsSemesterCourseException)
            {
                return Conflict(semesterCourseValidationException.GetInnerMessage());
            }
            catch (SemesterCourseValidationException semesterCourseValidationException)
            {
                return BadRequest(semesterCourseValidationException.GetInnerMessage());
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

        [HttpGet]
        public ActionResult<IQueryable<SemesterCourse>> GetAllSemesterCourses()
        {
            try
            {
                IQueryable storageSemesterCourses =
                    this.semesterCourseService.RetrieveAllSemesterCourses();

                return Ok(storageSemesterCourses);
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

        [HttpGet("{semesterCourseId}")]
        public async ValueTask<ActionResult<SemesterCourse>> GetSemesterCourseAsync(Guid semesterCourseId)
        {
            try
            {
                SemesterCourse storageSemesterCourse =
                    await this.semesterCourseService.RetrieveSemesterCourseByIdAsync(semesterCourseId);

                return Ok(storageSemesterCourse);
            }
            catch (SemesterCourseValidationException semesterSemesterCourseValidationException)
                when (semesterSemesterCourseValidationException.InnerException is NotFoundSemesterCourseException)
            {
                return NotFound(semesterSemesterCourseValidationException.GetInnerMessage());
            }
            catch (SemesterCourseValidationException semesterSemesterCourseValidationException)
            {
                return BadRequest(semesterSemesterCourseValidationException.GetInnerMessage());
            }
            catch (SemesterCourseDependencyException semesterSemesterCourseDependencyException)
            {
                return Problem(semesterSemesterCourseDependencyException.Message);
            }
            catch (SemesterCourseServiceException semesterSemesterCourseServiceException)
            {
                return Problem(semesterSemesterCourseServiceException.Message);
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
                return NotFound(semesterCourseValidationException.GetInnerMessage());
            }
            catch (SemesterCourseValidationException semesterCourseValidationException)
            {
                return BadRequest(semesterCourseValidationException.GetInnerMessage());
            }
            catch (SemesterCourseDependencyException semesterCourseDependencyException)
                when (semesterCourseDependencyException.InnerException is LockedSemesterCourseException)
            {
                return Locked(semesterCourseDependencyException.GetInnerMessage());
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
                    await this.semesterCourseService.RemoveSemesterCourseByIdAsync(semesterCourseId);

                return Ok(storageSemesterCourse);
            }
            catch (SemesterCourseValidationException semesterCourseValidationException)
                when (semesterCourseValidationException.InnerException is NotFoundSemesterCourseException)
            {
                return NotFound(semesterCourseValidationException.GetInnerMessage());
            }
            catch (SemesterCourseValidationException semesterCourseValidationException)
            {
                return BadRequest(semesterCourseValidationException.Message);
            }
            catch (SemesterCourseDependencyException semesterCourseDependencyException)
                when (semesterCourseDependencyException.InnerException is LockedSemesterCourseException)
            {
                return Locked(semesterCourseDependencyException.GetInnerMessage());
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

    }
}
