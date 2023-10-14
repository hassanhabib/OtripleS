// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
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
                Teacher createdTeacher =
                    await this.teacherService.CreateTeacherAsync(teacher);

                return Created(createdTeacher);
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
        public async ValueTask<ActionResult<Teacher>> GetTeacherByIdAsync(Guid teacherId)
        {
            try
            {
                Teacher teacher =
                    await this.teacherService.RetrieveTeacherByIdAsync(teacherId);

                return Ok(teacher);
            }
            catch (TeacherValidationException teacherValidationException)
                when (teacherValidationException.InnerException is NotFoundTeacherException)
            {
                return NotFound(teacherValidationException.GetInnerMessage());
            }
            catch (TeacherValidationException teacherValidationException)
            {
                return BadRequest(teacherValidationException.GetInnerMessage());
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
                return NotFound(teacherValidationException.GetInnerMessage());
            }
            catch (TeacherValidationException teacherValidationException)
            {
                return BadRequest(teacherValidationException.GetInnerMessage());
            }
            catch (TeacherDependencyException teacherDependencyException)
                when (teacherDependencyException.InnerException is LockedTeacherException)
            {
                return Locked(teacherDependencyException.GetInnerMessage());
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
                return NotFound(teacherValidationException.GetInnerMessage());
            }
            catch (TeacherValidationException teacherValidationException)
            {
                return BadRequest(teacherValidationException.Message);
            }
            catch (TeacherDependencyException teacherDependencyException)
                when (teacherDependencyException.InnerException is LockedTeacherException)
            {
                return Locked(teacherDependencyException.GetInnerMessage());
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

    }
}
