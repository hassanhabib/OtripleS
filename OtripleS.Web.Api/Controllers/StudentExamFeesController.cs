// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Foundations.StudentExamFees;
using OtripleS.Web.Api.Models.Foundations.StudentExamFees.Exceptions;
using OtripleS.Web.Api.Services.Foundations.StudentExamFees;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentExamFeesController : RESTFulController
    {
        private readonly IStudentExamFeeService studentExamFeeService;

        public StudentExamFeesController(IStudentExamFeeService studentExamFeeService) =>
            this.studentExamFeeService = studentExamFeeService;

        [HttpPost]
        public async ValueTask<ActionResult<StudentExamFee>> PostStudentExamFeeAsync(StudentExamFee studentExamFee)
        {
            try
            {
                StudentExamFee persistedStudentExamFee =
                    await this.studentExamFeeService.AddStudentExamFeeAsync(studentExamFee);

                return Ok(persistedStudentExamFee);
            }
            catch (StudentExamFeeValidationException studentExamFeeValidationException)
                when (studentExamFeeValidationException.InnerException is AlreadyExistsStudentExamFeeException)
            {
                string innerMessage = GetInnerMessage(studentExamFeeValidationException);

                return Conflict(innerMessage);
            }
            catch (StudentExamFeeValidationException studentExamFeeValidationException)
            {
                string innerMessage = GetInnerMessage(studentExamFeeValidationException);

                return BadRequest(innerMessage);
            }
            catch (StudentExamFeeDependencyException studentExamFeeDependencyException)
            {
                return Problem(studentExamFeeDependencyException.Message);
            }
            catch (StudentExamFeeServiceException studentExamFeeServiceException)
            {
                return Problem(studentExamFeeServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<StudentExamFee>> GetAllStudentExamFees()
        {
            try
            {
                IQueryable storageStudentExamFee =
                    this.studentExamFeeService.RetrieveAllStudentExamFees();

                return Ok(storageStudentExamFee);
            }
            catch (StudentExamFeeDependencyException studentExamFeeDependencyException)
            {
                return Problem(studentExamFeeDependencyException.Message);
            }
            catch (StudentExamFeeServiceException studentExamFeeServiceException)
            {
                return Problem(studentExamFeeServiceException.Message);
            }
        }

        [HttpGet("studentid/{studentId}/examfeeid/{examFeeId}/")]
        public async ValueTask<ActionResult<StudentExamFee>> GetStudentExamFeeByIdAsync(
            Guid studentId,
            Guid examFeeId)
        {
            try
            {
                StudentExamFee storageStudentExamFee =
                    await this.studentExamFeeService.RetrieveStudentExamFeeByIdsAsync(
                        studentId, examFeeId);

                return Ok(storageStudentExamFee);
            }
            catch (StudentExamFeeValidationException studentExamFeeValidationException)
                when (studentExamFeeValidationException.InnerException is NotFoundStudentExamFeeException)
            {
                string innerMessage = GetInnerMessage(studentExamFeeValidationException);

                return NotFound(innerMessage);
            }
            catch (StudentExamFeeValidationException studentExamFeeValidationException)
            {
                string innerMessage = GetInnerMessage(studentExamFeeValidationException);

                return BadRequest(innerMessage);
            }
            catch (StudentExamFeeDependencyException studentExamFeeDependencyException)
            {
                return Problem(studentExamFeeDependencyException.Message);
            }
            catch (StudentExamFeeServiceException studentExamFeeServiceException)
            {
                return Problem(studentExamFeeServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<StudentExamFee>> PutStudentExamFeeAsync(StudentExamFee studentExamFee)
        {
            try
            {
                StudentExamFee registeredStudentExamFee =
                    await this.studentExamFeeService.ModifyStudentExamFeeAsync(studentExamFee);

                return Ok(registeredStudentExamFee);
            }
            catch (StudentExamFeeValidationException studentExamFeeValidationException)
                when (studentExamFeeValidationException.InnerException is NotFoundStudentExamFeeException)
            {
                string innerMessage = GetInnerMessage(studentExamFeeValidationException);

                return NotFound(innerMessage);
            }
            catch (StudentExamFeeValidationException studentExamFeeValidationException)
            {
                string innerMessage = GetInnerMessage(studentExamFeeValidationException);

                return BadRequest(innerMessage);
            }
            catch (StudentExamFeeDependencyException studentExamFeeDependencyException)
                when (studentExamFeeDependencyException.InnerException is LockedStudentExamFeeException)
            {
                string innerMessage = GetInnerMessage(studentExamFeeDependencyException);

                return Locked(innerMessage);
            }
            catch (StudentExamFeeDependencyException studentExamFeeDependencyException)
            {
                return Problem(studentExamFeeDependencyException.Message);
            }
            catch (StudentExamFeeServiceException studentExamFeeServiceException)
            {
                return Problem(studentExamFeeServiceException.Message);
            }
        }

        [HttpDelete("studentid/{studentId}/examfeeid/{examFeeId}/")]
        public async ValueTask<ActionResult<StudentExamFee>> DeleteStudentExamFeeAsync(
            Guid studentId,
            Guid examFeeId)
        {
            try
            {
                StudentExamFee storageStudentExamFee =
                    await this.studentExamFeeService.RemoveStudentExamFeeByIdAsync(
                        studentId,
                        examFeeId);

                return Ok(storageStudentExamFee);
            }
            catch (StudentExamFeeValidationException studentExamFeeValidationException)
                when (studentExamFeeValidationException.InnerException is NotFoundStudentExamFeeException)
            {
                string innerMessage = GetInnerMessage(studentExamFeeValidationException);

                return NotFound(innerMessage);
            }
            catch (StudentExamFeeValidationException studentExamFeeValidationException)
            {
                string innerMessage = GetInnerMessage(studentExamFeeValidationException);

                return BadRequest(studentExamFeeValidationException);
            }
            catch (StudentExamFeeDependencyException studentExamFeeDependencyException)
               when (studentExamFeeDependencyException.InnerException is LockedStudentExamFeeException)
            {
                string innerMessage = GetInnerMessage(studentExamFeeDependencyException);

                return Locked(innerMessage);
            }
            catch (StudentExamFeeDependencyException studentExamFeeDependencyException)
            {
                return Problem(studentExamFeeDependencyException.Message);
            }
            catch (StudentExamFeeServiceException studentExamFeeServiceException)
            {
                return Problem(studentExamFeeServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}