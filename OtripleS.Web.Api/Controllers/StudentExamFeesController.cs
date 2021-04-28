// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;
using OtripleS.Web.Api.Services.StudentExamFees;
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

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}