// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;
using OtripleS.Web.Api.Services.Foundations.StudentRegistrations;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentRegistrationsController : RESTFulController
    {
        private readonly IStudentRegistrationService studentRegistrationService;

        public StudentRegistrationsController(IStudentRegistrationService studentRegistrationService) =>
            this.studentRegistrationService = studentRegistrationService;

        [HttpPost]
        public async ValueTask<ActionResult<StudentRegistration>> PostStudentRegistrationAsync(
            StudentRegistration studentRegistration)
        {
            try
            {
                StudentRegistration addedStudentRegistration =
                    await this.studentRegistrationService.AddStudentRegistrationAsync(studentRegistration);

                return Created(addedStudentRegistration);
            }
            catch (StudentRegistrationValidationException studentRegistrationValidationException)
                when (studentRegistrationValidationException.InnerException is AlreadyExistsStudentRegistrationException)
            {
                return Conflict(studentRegistrationValidationException.GetInnerMessage());
            }
            catch (StudentRegistrationValidationException studentRegistrationValidationException)
            {
                return BadRequest(studentRegistrationValidationException.GetInnerMessage());
            }
            catch (StudentRegistrationDependencyException studentRegistrationDependencyException)
            {
                return Problem(studentRegistrationDependencyException.Message);
            }
            catch (StudentRegistrationServiceException studentRegistrationServiceException)
            {
                return Problem(studentRegistrationServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<StudentRegistration>> GetAllStudentRegistrations()
        {
            try
            {
                IQueryable storageStudentRegistrations =
                    this.studentRegistrationService.RetrieveAllStudentRegistrations();

                return Ok(storageStudentRegistrations);
            }
            catch (StudentRegistrationDependencyException studentRegistrationDependencyException)
            {
                return Problem(studentRegistrationDependencyException.Message);
            }
            catch (StudentRegistrationServiceException studentRegistrationServiceException)
            {
                return Problem(studentRegistrationServiceException.Message);
            }
        }

        [HttpGet("students/{studentId}/registrations/{registrationId}")]
        public async ValueTask<ActionResult<StudentRegistration>> GetStudentRegistrationAsync(
            Guid studentId,
            Guid registrationId)
        {
            try
            {
                StudentRegistration storageStudentRegistration =
                    await this.studentRegistrationService.RetrieveStudentRegistrationByIdAsync(studentId, registrationId);

                return Ok(storageStudentRegistration);
            }
            catch (StudentRegistrationValidationException semesterStudentRegistrationValidationException)
                when (semesterStudentRegistrationValidationException.InnerException is NotFoundStudentRegistrationException)
            {
                return NotFound(semesterStudentRegistrationValidationException.GetInnerMessage());
            }
            catch (StudentRegistrationValidationException semesterStudentRegistrationValidationException)
            {
                return BadRequest(semesterStudentRegistrationValidationException.GetInnerMessage());
            }
            catch (StudentRegistrationDependencyException semesterStudentRegistrationDependencyException)
            {
                return Problem(semesterStudentRegistrationDependencyException.Message);
            }
            catch (StudentRegistrationServiceException semesterStudentRegistrationServiceException)
            {
                return Problem(semesterStudentRegistrationServiceException.Message);
            }
        }

        [HttpDelete("students/{studentId}/registrations/{registrationId}")]
        public async ValueTask<ActionResult<bool>> DeleteStudentRegistrationAsync(Guid studentId, Guid registrationId)
        {
            try
            {
                StudentRegistration deletedStudentRegistration =
                    await this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                        studentId,
                        registrationId);

                return Ok(deletedStudentRegistration);
            }
            catch (StudentRegistrationValidationException studentRegistrationValidationException)
                when (studentRegistrationValidationException.InnerException is NotFoundStudentRegistrationException)
            {
                return NotFound(studentRegistrationValidationException.GetInnerMessage());
            }
            catch (StudentRegistrationValidationException studentRegistrationValidationException)
            {
                return BadRequest(studentRegistrationValidationException.GetInnerMessage());
            }
            catch (StudentRegistrationDependencyException studentRegistrationValidationException)
               when (studentRegistrationValidationException.InnerException is LockedStudentRegistrationException)
            {
                return Locked(studentRegistrationValidationException.GetInnerMessage());
            }
            catch (StudentRegistrationDependencyException studentRegistrationDependencyException)
            {
                return Problem(studentRegistrationDependencyException.Message);
            }
            catch (StudentRegistrationServiceException studentRegistrationServiceException)
            {
                return Problem(studentRegistrationServiceException.Message);
            }
        }

    }
}
