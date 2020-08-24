// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using OtripleS.Web.Api.Services.StudentSemesterCourses;
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
                StudentSemesterCourse persistedStudentSemesterCourse =
                    await this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(studentSemesterCourse);

                return Ok(persistedStudentSemesterCourse);
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
                when (studentSemesterCourseValidationException.InnerException is AlreadyExistsStudentSemesterCourseException)
            {
                string innerMessage = GetInnerMessage(studentSemesterCourseValidationException);

                return Conflict(innerMessage);
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
            {
                string innerMessage = GetInnerMessage(studentSemesterCourseValidationException);

                return BadRequest(innerMessage);
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
                string innerMessage = GetInnerMessage(studentSemesterCourseValidationException);

                return NotFound(innerMessage);
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
            {
                string innerMessage = GetInnerMessage(studentSemesterCourseValidationException);

                return BadRequest(innerMessage);
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

        [HttpDelete("students/{studentId}/semesters/{semesterId}")]
        public async ValueTask<ActionResult<StudentSemesterCourse>>
        DeleteStudentSemesterCourseAsync(Guid semesterCourseId, Guid studentId)
        {
            try
            {
                StudentSemesterCourse storageStudentSemesterCourse =
                    await this.studentSemesterCourseService.DeleteStudentSemesterCourseAsync(semesterCourseId, studentId);

                return Ok(storageStudentSemesterCourse);
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
                when (studentSemesterCourseValidationException.InnerException is NotFoundStudentSemesterCourseException)
            {
                string innerMessage = GetInnerMessage(studentSemesterCourseValidationException);

                return NotFound(innerMessage);
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
            {
                return BadRequest(studentSemesterCourseValidationException.Message);
            }
            catch (StudentSemesterCourseDependencyException studentSemesterCourseDependencyException)
                when (studentSemesterCourseDependencyException.InnerException is LockedStudentSemesterCourseException)
            {
                string innerMessage = GetInnerMessage(studentSemesterCourseDependencyException);

                return Locked(innerMessage);
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
                string innerMessage = GetInnerMessage(studentSemesterCourseValidationException);

                return NotFound(innerMessage);
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
            {
                string innerMessage = GetInnerMessage(studentSemesterCourseValidationException);

                return BadRequest(innerMessage);
            }
            catch (StudentSemesterCourseDependencyException studentSemesterCourseDependencyException)
                when (studentSemesterCourseDependencyException.InnerException is LockedStudentSemesterCourseException)
            {
                string innerMessage = GetInnerMessage(studentSemesterCourseDependencyException);

                return Locked(innerMessage);
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

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
