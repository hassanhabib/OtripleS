// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Classrooms;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassroomsController : RESTFulController
    {
        private readonly IClassroomService classroomService;

        public ClassroomsController(IClassroomService classroomService) =>
            this.classroomService = classroomService;

        [HttpPost]
        public async ValueTask<ActionResult<Classroom>> PostClassroomAsync(Classroom classroom)
        {
            try
            {
                Classroom createdClassroom =
                    await this.classroomService.CreateClassroomAsync(classroom);

                return Created(createdClassroom);
            }
            catch (ClassroomValidationException classroomValidationException)
                when (classroomValidationException.InnerException is AlreadyExistsClassroomException)
            {
                return Conflict(classroomValidationException.GetInnerMessage());
            }
            catch (ClassroomValidationException classroomValidationException)
            {
                return BadRequest(classroomValidationException.InnerException);
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

        [HttpGet]
        public ActionResult<IQueryable<Classroom>> GetAllClassrooms()
        {
            try
            {
                IQueryable storageClassrooms =
                    this.classroomService.RetrieveAllClassrooms();

                return Ok(storageClassrooms);
            }
            catch (ClassroomDependencyException classRoomDependencyException)
            {
                return Problem(classRoomDependencyException.Message);
            }
            catch (ClassroomServiceException classRoomServiceException)
            {
                return Problem(classRoomServiceException.Message);
            }
        }

        [HttpGet("{classroomId}")]
        public async ValueTask<ActionResult<Classroom>> GetClassroomByIdAsync(Guid classroomId)
        {
            try
            {
                Classroom classroom =
                    await this.classroomService.RetrieveClassroomById(classroomId);

                return Ok(classroom);
            }
            catch (ClassroomValidationException classroomValidationException)
                when (classroomValidationException.InnerException is NotFoundClassroomException)
            {
                return NotFound(classroomValidationException.GetInnerMessage());
            }
            catch (ClassroomValidationException classroomValidationException)
            {
                return BadRequest(classroomValidationException.GetInnerMessage());
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

        [HttpPut]
        public async ValueTask<ActionResult<Classroom>> PutClassroomAsync(Classroom classroom)
        {
            try
            {
                Classroom registeredClassroom =
                    await this.classroomService.ModifyClassroomAsync(classroom);

                return Ok(registeredClassroom);
            }
            catch (ClassroomValidationException classroomValidationException)
                when (classroomValidationException.InnerException is NotFoundClassroomException)
            {
                return NotFound(classroomValidationException.GetInnerMessage());
            }
            catch (ClassroomValidationException classroomValidationException)
            {
                return BadRequest(classroomValidationException.InnerException);
            }
            catch (ClassroomDependencyException classroomDependencyException)
                when (classroomDependencyException.InnerException is LockedClassroomException)
            {
                return Locked(classroomDependencyException.GetInnerMessage());
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
        public async ValueTask<ActionResult<Classroom>> DeleteClassroomAsync(Guid classroomId)
        {
            try
            {
                Classroom storageClassroom =
                    await this.classroomService.RemoveClassroomAsync(classroomId);

                return Ok(storageClassroom);
            }
            catch (ClassroomValidationException classroomValidationException)
                when (classroomValidationException.InnerException is NotFoundClassroomException)
            {
                return NotFound(classroomValidationException.GetInnerMessage());
            }
            catch (ClassroomValidationException classroomValidationException)
            {
                return BadRequest(classroomValidationException.Message);
            }
            catch (ClassroomDependencyException classroomDependencyException)
                when (classroomDependencyException.InnerException is LockedClassroomException)
            {
                return Locked(classroomDependencyException.GetInnerMessage());
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

    }
}
