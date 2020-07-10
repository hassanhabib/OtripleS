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
            
            
        [HttpPut]
        public async ValueTask<ActionResult<Teacher>> PutTeacher(Teacher teacher)
        {
            try
            {
                Teacher updatedTeacher =
                    await this.teacherService.ModifyTeacherAsync(teacher);

                return Ok(updatedTeacher);
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
        
        [HttpGet]
        public ActionResult<IQueryable<Teacher>> GetAllTeachers()
        {
            try
            {
                IQueryable<Teacher> teachers =
                    this.teacherService.RetrieveAllTeachers();

                return Ok(teachers);
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