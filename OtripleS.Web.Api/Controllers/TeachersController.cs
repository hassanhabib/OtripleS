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
using OtripleS.Web.Api.Services;
using OtripleS.Web.Api.Services.Teachers;
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

        [HttpDelete("{teacherId}")]
        public async ValueTask<ActionResult<Teacher>> DeleteTeacherAsync(Guid teacherId)
        {
            try
            {
                Teacher storageTeacher =
                    await this.teacherService.DeleteTeacherByIdAsync(teacherId);

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
                string innerMessage = GetInnerMessage(teacherValidationException);

                return BadRequest(teacherValidationException);
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

        public static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}