// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using OtripleS.Web.Api.Services.Foundations.StudentGuardians;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentGuardiansController : RESTFulController
    {
        private readonly IStudentGuardianService studentGuardianService;

        public StudentGuardiansController(IStudentGuardianService studentGuardianService) =>
            this.studentGuardianService = studentGuardianService;

        [HttpPost]
        public async ValueTask<ActionResult<StudentGuardian>> PostStudentGuardianAsync(StudentGuardian studentGuardian)
        {
            try
            {
                StudentGuardian addedStudentGuardian =
                    await studentGuardianService.AddStudentGuardianAsync(studentGuardian);

                return Created(addedStudentGuardian);
            }
            catch (StudentGuardianValidationException studentGuardianValidationException)
                when (studentGuardianValidationException.InnerException is AlreadyExistsStudentGuardianException)
            {
                string innerMessage = GetInnerMessage(studentGuardianValidationException);

                return Conflict(innerMessage);
            }
            catch (StudentGuardianValidationException studentGuardianValidationException)
            {
                string innerMessage = GetInnerMessage(studentGuardianValidationException);

                return BadRequest(innerMessage);
            }
            catch (StudentGuardianDependencyException studentGuardianDependencyException)
            {
                return Problem(studentGuardianDependencyException.Message);
            }
            catch (StudentGuardianServiceException studentGuardianServiceException)
            {
                return Problem(studentGuardianServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<StudentGuardian>> GetAllStudentGuardians()
        {
            try
            {
                IQueryable<StudentGuardian> storageStudentGuardians = studentGuardianService.RetrieveAllStudentGuardians();

                return Ok(storageStudentGuardians);
            }
            catch (StudentGuardianDependencyException studentGuardianDependencyException)
            {
                return Problem(studentGuardianDependencyException.Message);
            }
            catch (StudentGuardianServiceException studentGuardianServiceException)
            {
                return Problem(studentGuardianServiceException.Message);
            }
        }

        [HttpGet("students/{studentId}/guardians/{guardianId}")]
        public async ValueTask<ActionResult<StudentGuardian>> GetStudentGuardianByIdAsync(Guid studentId, Guid guardianId)
        {
            try
            {
                StudentGuardian storageStudentGuardian =
                    await this.studentGuardianService.RetrieveStudentGuardianByIdAsync(studentId, guardianId);

                return Ok(storageStudentGuardian);
            }
            catch (StudentGuardianValidationException studentGuardianValidationException)
                when (studentGuardianValidationException.InnerException is NotFoundStudentGuardianException)
            {
                string innerMessage = GetInnerMessage(studentGuardianValidationException);

                return NotFound(innerMessage);
            }
            catch (StudentGuardianValidationException studentGuardianValidationException)
            {
                string innerMessage = GetInnerMessage(studentGuardianValidationException);

                return BadRequest(innerMessage);
            }
            catch (StudentGuardianDependencyException studentGuardianDependencyException)
            {
                return Problem(studentGuardianDependencyException.Message);
            }
            catch (StudentGuardianServiceException studentGuardianServiceException)
            {
                return Problem(studentGuardianServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<StudentGuardian>> PutStudentGuardianAsync(StudentGuardian studentGuardian)
        {
            try
            {
                StudentGuardian registeredStudentGuardian =
                    await studentGuardianService.ModifyStudentGuardianAsync(studentGuardian);

                return Ok(registeredStudentGuardian);
            }
            catch (StudentGuardianValidationException studentGuardianValidationException)
                when (studentGuardianValidationException.InnerException is NotFoundStudentGuardianException)
            {
                string innerMessage = GetInnerMessage(studentGuardianValidationException);

                return NotFound(innerMessage);
            }
            catch (StudentGuardianValidationException studentGuardianValidationException)
            {
                string innerMessage = GetInnerMessage(studentGuardianValidationException);

                return BadRequest(innerMessage);
            }
            catch (StudentGuardianDependencyException studentGuardianDependencyException)
                when (studentGuardianDependencyException.InnerException is LockedStudentGuardianException)
            {
                string innerMessage = GetInnerMessage(studentGuardianDependencyException);

                return Locked(innerMessage);
            }
            catch (StudentGuardianDependencyException studentGuardianDependencyException)
            {
                return Problem(studentGuardianDependencyException.Message);
            }
            catch (StudentGuardianServiceException studentGuardianServiceException)
            {
                return Problem(studentGuardianServiceException.Message);
            }
        }

        [HttpDelete("students/{studentId}/guardians/{guardianId}")]
        public async ValueTask<ActionResult<bool>> DeleteStudentGuardianAsync(Guid studentId, Guid guardianId)
        {
            try
            {
                StudentGuardian storageStudentGuardian =
                    await this.studentGuardianService.RemoveStudentGuardianByIdsAsync(studentId, guardianId);

                return Ok(storageStudentGuardian);
            }
            catch (StudentGuardianValidationException studentGuardianValidationException)
                when (studentGuardianValidationException.InnerException is NotFoundStudentGuardianException)
            {
                string innerMessage = GetInnerMessage(studentGuardianValidationException);

                return NotFound(innerMessage);
            }
            catch (StudentGuardianValidationException studentGuardianValidationException)
            {
                string innerMessage = GetInnerMessage(studentGuardianValidationException);

                return BadRequest(innerMessage);
            }
            catch (StudentGuardianDependencyException studentGuardianValidationException)
               when (studentGuardianValidationException.InnerException is LockedStudentGuardianException)
            {
                string innerMessage = GetInnerMessage(studentGuardianValidationException);

                return Locked(innerMessage);
            }
            catch (StudentGuardianDependencyException studentGuardianDependencyException)
            {
                return Problem(studentGuardianDependencyException.Message);
            }
            catch (StudentGuardianServiceException studentGuardianServiceException)
            {
                return Problem(studentGuardianServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
