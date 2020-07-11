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

        [HttpGet("{teacherId}")]
        public async ValueTask<ActionResult<Teacher>> GetById(Guid teacherId)
        {
            try
            {
                Teacher teacher = 
                await this.teacherService.RetrieveTeacherByIdAsync(teacherId);

                return Ok(teacherId);
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
            catch (TeacherDependencyException teacherValidationException)
            {
                return Problem(teacherValidationException.Message);
            }
            catch (TeacherServiceException teacherValidationException)
            {
                return Problem(teacherValidationException.Message);
            }
        }

        public static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
