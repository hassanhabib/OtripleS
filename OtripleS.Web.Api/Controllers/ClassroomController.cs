using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using OtripleS.Web.Api.Services.Classrooms;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomController : RESTFulController
    {
        private readonly IClassroomService classroomService;

        public ClassroomController(IClassroomService classroomService)
        {
            this.classroomService = classroomService;
        }
        
        [HttpGet("{courseId}")]
        public async ValueTask<ActionResult<Classroom>> GetCourseAsync(Guid classroomId)
        {
            try
            {
                Classroom storageClassroom =
                    await this.classroomService.GetClassroomById(classroomId);

                return Ok(storageClassroom);
            }
            catch (ClassroomValidationException classroomValidationException)
                when (classroomValidationException.InnerException is NotFoundClassroomException)
            {
                string innerMessage = GetInnerMessage(classroomValidationException);

                return NotFound(innerMessage);
            }
            catch (ClassroomValidationException classroomValidationException)
            {
                return BadRequest(classroomValidationException.Message);
            }
            catch (ClassroomDependencyException classroomDependencyException)
                when (classroomDependencyException.InnerException is LockedClassroomException)
            {
                string innerMessage = GetInnerMessage(classroomDependencyException);

                return Locked(innerMessage);
            }
            catch (ClassroomDependencyException classroomDependencyException)
            {
                return Problem(classroomDependencyException.Message);
            }
            catch (ClassroomServiceException classroomServiceException)
            {
                return Problem(classroomServiceException.Message);
            }
        }
        
        [HttpDelete("{classroomId}")]
        public async ValueTask<ActionResult<Classroom>> DeleteCourseAsync(Guid classroomId)
        {
            try
            {
                Classroom storageClassroom =
                    await this.classroomService.DeleteClassroomAsync(classroomId);

                return Ok(storageClassroom);
            }
            catch (ClassroomValidationException classroomValidationException)
                when (classroomValidationException.InnerException is NotFoundClassroomException)
            {
                string innerMessage = GetInnerMessage(classroomValidationException);

                return NotFound(innerMessage);
            }
            catch (ClassroomValidationException classroomValidationException)
            {
                return BadRequest(classroomValidationException.Message);
            }
            catch (ClassroomDependencyException classroomDependencyException)
                when (classroomDependencyException.InnerException is LockedClassroomException)
            {
                string innerMessage = GetInnerMessage(classroomDependencyException);

                return Locked(innerMessage);
            }
            catch (ClassroomDependencyException classroomDependencyException)
            {
                return Problem(classroomDependencyException.Message);
            }
            catch (ClassroomServiceException classroomServiceException)
            {
                return Problem(classroomServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}