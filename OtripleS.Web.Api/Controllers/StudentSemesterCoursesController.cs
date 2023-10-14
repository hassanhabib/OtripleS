// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using OtripleS.Web.Api.Services.Foundations.StudentSemesterCourses;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentSemesterCoursesController : RESTFulController
    {
        private readonly IStudentSemesterCourseService studentSemesterCourseService;

        public StudentSemesterCoursesController(IStudentSemesterCourseService studentSemesterCourseService) =>
            this.studentSemesterCourseService = studentSemesterCourseService;

        [HttpPost]
        public async ValueTask<ActionResult<StudentSemesterCourse>>
        PostStudentSemesterCourseAsync(StudentSemesterCourse studentSemesterCourse)
        {
            try
            {
                StudentSemesterCourse createdStudentSemesterCourse =
                    await this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(studentSemesterCourse);

                return Created(createdStudentSemesterCourse);
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
                when (studentSemesterCourseValidationException.InnerException is AlreadyExistsStudentSemesterCourseException)
            {
                return Conflict(studentSemesterCourseValidationException.GetInnerMessage());
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
            {
                return BadRequest(studentSemesterCourseValidationException.GetInnerMessage());
            }
            catch (StudentSemesterCourseDependencyException studentSemesterCourseDependencyException)
            {
                return Problem(studentSemesterCourseDependencyException.Message);
            }
            catch (StudentSemesterCourseServiceException studentSemesterCourseServiceException)
            {
                return Problem(studentSemesterCourseServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<StudentSemesterCourse>> GetAllStudentSemesterCourse()
        {
            try
            {
                IQueryable storageStudentSemesterCourses =
                    this.studentSemesterCourseService.RetrieveAllStudentSemesterCourses();

                return Ok(storageStudentSemesterCourses);
            }
            catch (StudentSemesterCourseDependencyException studentSemesterCourseDependencyException)
            {
                return Problem(studentSemesterCourseDependencyException.Message);
            }
            catch (StudentSemesterCourseServiceException studentSemesterCourseServiceException)
            {
                return Problem(studentSemesterCourseServiceException.Message);
            }
        }

        [HttpGet("students/{studentId}/semesters/{semesterId}")]
        public async ValueTask<ActionResult<StudentSemesterCourse>>
        GetStudentSemesterCourseByIdAsync(Guid semesterCourseId, Guid studentId)
        {
            try
            {
                StudentSemesterCourse storageStudentSemesterCourse =
                    await this.studentSemesterCourseService.RetrieveStudentSemesterCourseByIdAsync(semesterCourseId, studentId);

                return Ok(storageStudentSemesterCourse);
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
                when (studentSemesterCourseValidationException.InnerException is NotFoundStudentSemesterCourseException)
            {
                return NotFound(studentSemesterCourseValidationException.GetInnerMessage());
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
            {
                return BadRequest(studentSemesterCourseValidationException.GetInnerMessage());
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<StudentSemesterCourse>>
        PutStudentSemesterCourseAsync(StudentSemesterCourse studentSemesterCourse)
        {
            try
            {
                StudentSemesterCourse registeredStudentSemesterCourse =
                    await this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(studentSemesterCourse);

                return Ok(registeredStudentSemesterCourse);
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
                when (studentSemesterCourseValidationException.InnerException is NotFoundStudentSemesterCourseException)
            {
                return NotFound(studentSemesterCourseValidationException.GetInnerMessage());
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
            {
                return BadRequest(studentSemesterCourseValidationException.GetInnerMessage());
            }
            catch (StudentSemesterCourseDependencyException studentSemesterCourseDependencyException)
                when (studentSemesterCourseDependencyException.InnerException is LockedStudentSemesterCourseException)
            {
                return Locked(studentSemesterCourseDependencyException.GetInnerMessage());
            }
            catch (StudentSemesterCourseDependencyException studentSemesterCourseDependencyException)
            {
                return Problem(studentSemesterCourseDependencyException.Message);
            }
            catch (StudentSemesterCourseServiceException studentSemesterCourseServiceException)
            {
                return Problem(studentSemesterCourseServiceException.Message);
            }
        }

        [HttpDelete("students/{studentId}/semesters/{semesterId}")]
        public async ValueTask<ActionResult<StudentSemesterCourse>>
        DeleteStudentSemesterCourseAsync(Guid semesterCourseId, Guid studentId)
        {
            try
            {
                StudentSemesterCourse storageStudentSemesterCourse =
                    await this.studentSemesterCourseService.RemoveStudentSemesterCourseByIdsAsync(
                        semesterCourseId,
                        studentId);

                return Ok(storageStudentSemesterCourse);
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
                when (studentSemesterCourseValidationException.InnerException is NotFoundStudentSemesterCourseException)
            {
                return NotFound(studentSemesterCourseValidationException.GetInnerMessage());
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
            {
                return BadRequest(studentSemesterCourseValidationException.Message);
            }
            catch (StudentSemesterCourseDependencyException studentSemesterCourseDependencyException)
                when (studentSemesterCourseDependencyException.InnerException is LockedStudentSemesterCourseException)
            {
                return Locked(studentSemesterCourseDependencyException.GetInnerMessage());
            }
        }

    }
}
