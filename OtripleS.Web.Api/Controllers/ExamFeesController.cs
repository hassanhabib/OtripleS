// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.ExamFees.Exceptions;
using OtripleS.Web.Api.Services.ExamFees;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamFeesController : RESTFulController
    {
        private readonly IExamFeeService examFeeService;

        public ExamFeesController(IExamFeeService examFeeService) =>
            this.examFeeService = examFeeService;

        [HttpPost]
        public async ValueTask<ActionResult<ExamFee>> PostExamFeeAsync(ExamFee examFee)
        {
            try
            {
                ExamFee persistedExamFee =
                    await this.examFeeService.AddExamFeeAsync(examFee);

                return Ok(persistedExamFee);
            }
            catch (ExamFeeValidationException examFeeValidationException)
                when (examFeeValidationException.InnerException is AlreadyExistsExamFeeException)
            {
                string innerMessage = GetInnerMessage(examFeeValidationException);

                return Conflict(innerMessage);
            }
            catch (ExamFeeValidationException examFeeValidationException)
            {
                string innerMessage = GetInnerMessage(examFeeValidationException);

                return BadRequest(innerMessage);
            }
            catch (ExamFeeDependencyException examFeeDependencyException)
            {
                return Problem(examFeeDependencyException.Message);
            }
            catch (ExamFeeServiceException examFeeServiceException)
            {
                return Problem(examFeeServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<ExamFee>> GetAllExamFees()
        {
            try
            {
                IQueryable storageExamFee =
                    this.examFeeService.RetrieveAllExamFees();

                return Ok(storageExamFee);
            }
            catch (ExamFeeDependencyException examFeeDependencyException)
            {
                return Problem(examFeeDependencyException.Message);
            }
            catch (ExamFeeServiceException examFeeServiceException)
            {
                return Problem(examFeeServiceException.Message);
            }
        }

        [HttpGet("{examFeeId}")]
        public async ValueTask<ActionResult<ExamFee>> GetExamFeeAsync(Guid examFeeId)
        {
            try
            {
                ExamFee storageExamFee =
                    await this.examFeeService.RetrieveExamFeeByIdAsync(examFeeId);

                return Ok(storageExamFee);
            }
            catch (ExamFeeValidationException examFeeValidationException)
                when (examFeeValidationException.InnerException is NotFoundExamFeeException)
            {
                string innerMessage = GetInnerMessage(examFeeValidationException);

                return NotFound(innerMessage);
            }
            catch (ExamFeeValidationException examFeeValidationException)
            {
                string innerMessage = GetInnerMessage(examFeeValidationException);

                return BadRequest(innerMessage);
            }
            catch (ExamFeeDependencyException examFeeDependencyException)
            {
                return Problem(examFeeDependencyException.Message);
            }
            catch (ExamFeeServiceException examFeeServiceException)
            {
                return Problem(examFeeServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}