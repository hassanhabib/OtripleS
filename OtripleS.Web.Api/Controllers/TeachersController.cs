// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Teachers;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController : RESTFulController
    {
        private readonly ITeacherService teacherService;

        public TeachersController(ITeacherService teacherService) =>
            this.teacherService = teacherService;

        [HttpPost]
        public async ValueTask<ActionResult<Teacher>> PostTeacherAsync(Teacher teacher)
        {
            try
            {
                Teacher registeredTeacher =
                    await this.teacherService.CreateTeacherAsync(teacher);

                return Created(registeredTeacher);
            }
            catch (TeacherValidationException teacherValidationException)
                when (teacherValidationException.InnerException is AlreadyExistsTeacherException)
            {
                return Conflict(teacherValidationException.InnerException);
            }
            catch (TeacherValidationException teacherValidationException)
            {
                return BadRequest(teacherValidationException.InnerException);
            }
            catch (TeacherDependencyException teacherDependencyException)
            {
                return InternalServerError(teacherDependencyException);
            }
            catch (TeacherServiceException teacherServiceException)
            {
                return InternalServerError(teacherServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Teacher>> GetAllTeachers()
        {
            try
            {
                IQueryable storageTeacher =
                    this.teacherService.RetrieveAllTeachers();

                return Ok(storageTeacher);
            }
            catch (TeacherDependencyException teacherDependencyException)
            {
                return Problem(teacherDependencyException.Message);
            }
            catch (TeacherServiceException teacherServiceException)
            {
                return Problem(teacherServiceException.Message);
            }
        }

        [HttpGet("{teacherId}")]
        public async ValueTask<ActionResult<Teacher>> GetTeacherByIdAsync(Guid TeacherId)
        {
            try
            {
                Teacher teacher =
                    await this.teacherService.RetrieveTeacherByIdAsync(TeacherId);

                return Ok(teacher);
            }
            catch (TeacherValidationException teacherValidationException)
                when (teacherValidationException.InnerException is NotFoundTeacherException)
            {
                string innerMessage = GetInnerMessage(teacherValidationException);

                return NotFound(innerMessage);
            }
            catch (TeacherValidationException teacherValidationException)
            {
                string innerMessage = GetInnerMessage(teacherValidationException);

                return BadRequest(innerMessage);
            }
            catch (TeacherDependencyException teacherDependencyException)
            {
                return Problem(teacherDependencyException.Message);
            }
            catch (TeacherServiceException teacherServiceException)
            {
                return Problem(teacherServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Teacher>> PutTeacherAsync(Teacher teacher)
        {
            try
            {
                Teacher registeredTeacher =
                    await this.teacherService.ModifyTeacherAsync(teacher);

                return Ok(registeredTeacher);
            }
            catch (TeacherValidationException teacherValidationException)
                when (teacherValidationException.InnerException is NotFoundTeacherException)
            {
                string innerMessage = GetInnerMessage(teacherValidationException);

                return NotFound(innerMessage);
            }
            catch (TeacherValidationException teacherValidationException)
            {
                string innerMessage = GetInnerMessage(teacherValidationException);

                return BadRequest(innerMessage);
            }
            catch (TeacherDependencyException teacherDependencyException)
                when (teacherDependencyException.InnerException is LockedTeacherException)
            {
                string innerMessage = GetInnerMessage(teacherDependencyException);

                return Locked(innerMessage);
            }
            catch (TeacherDependencyException teacherDependencyException)
            {
                return Problem(teacherDependencyException.Message);
            }
            catch (TeacherServiceException teacherServiceException)
            {
                return Problem(teacherServiceException.Message);
            }
        }

        [HttpDelete("{teacherId}")]
        public async ValueTask<ActionResult<Teacher>> DeleteTeacherAsync(Guid teacherId)
        {
            try
            {
                Teacher storageTeacher =
                    await this.teacherService.RemoveTeacherByIdAsync(teacherId);

                return Ok(storageTeacher);
            }
            catch (TeacherValidationException teacherValidationException)
                when (teacherValidationException.InnerException is NotFoundTeacherException)
            {
                string innerMessage = GetInnerMessage(teacherValidationException);

                return NotFound(innerMessage);
            }
            catch (TeacherValidationException teacherValidationException)
            {
                return BadRequest(teacherValidationException.Message);
            }
            catch (TeacherDependencyException teacherDependencyException)
                when (teacherDependencyException.InnerException is LockedTeacherException)
            {
                string innerMessage = GetInnerMessage(teacherDependencyException);

                return Locked(innerMessage);
            }
            catch (TeacherDependencyException teacherDependencyException)
            {
                return Problem(teacherDependencyException.Message);
            }
            catch (TeacherServiceException teacherServiceException)
            {
                return Problem(teacherServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
