// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.ExamFees.Exceptions;
using OtripleS.Web.Api.Services.Foundations.ExamFees;
using RESTFulSense.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

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

                return Created(persistedExamFee);
            }
            catch (ExamFeeValidationException examFeeValidationException)
                when (examFeeValidationException.InnerException is AlreadyExistsExamFeeException)
            {
                return Conflict(examFeeValidationException.InnerException);
            }
            catch (ExamFeeValidationException examFeeValidationException)
            {
                return BadRequest(examFeeValidationException.InnerException);
            }
            catch (ExamFeeDependencyException examFeeDependencyException)
            {
                return InternalServerError(examFeeDependencyException);
            }
            catch (ExamFeeServiceException examFeeServiceException)
            {
                return InternalServerError(examFeeServiceException);
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
                return InternalServerError(examFeeDependencyException);
            }
            catch (ExamFeeServiceException examFeeServiceException)
            {
                return InternalServerError(examFeeServiceException);
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
                return NotFound(examFeeValidationException.InnerException);
            }
            catch (ExamFeeValidationException examFeeValidationException)
            {
                return BadRequest(examFeeValidationException.InnerException);
            }
            catch (ExamFeeDependencyException examFeeDependencyException)
            {
                return InternalServerError(examFeeDependencyException);
            }
            catch (ExamFeeServiceException examFeeServiceException)
            {
                return InternalServerError(examFeeServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<ExamFee>> PutExamFeeAsync(ExamFee examFee)
        {
            try
            {
                ExamFee registeredExamFee =
                    await this.examFeeService.ModifyExamFeeAsync(examFee);

                return Ok(registeredExamFee);
            }
            catch (ExamFeeValidationException examFeeValidationException)
                when (examFeeValidationException.InnerException is NotFoundExamFeeException)
            {
                return NotFound(examFeeValidationException.InnerException);
            }
            catch (ExamFeeValidationException examFeeValidationException)
            {
                return BadRequest(examFeeValidationException.InnerException);
            }
            catch (ExamFeeDependencyException examFeeDependencyException)
                when (examFeeDependencyException.InnerException is LockedExamFeeException)
            {
                return Locked(examFeeDependencyException.InnerException);
            }
            catch (ExamFeeDependencyException examFeeDependencyException)
            {
                return InternalServerError(examFeeDependencyException);
            }
            catch (ExamFeeServiceException examFeeServiceException)
            {
                return InternalServerError(examFeeServiceException);
            }
        }

        [HttpDelete("{examFeeId}")]
        public async ValueTask<ActionResult<ExamFee>> DeleteExamFeeAsync(Guid examFeeId)
        {
            try
            {
                ExamFee storageExamFee =
                    await this.examFeeService.RemoveExamFeeByIdAsync(examFeeId);

                return Ok(storageExamFee);
            }
            catch (ExamFeeValidationException examFeeValidationException)
                when (examFeeValidationException.InnerException is NotFoundExamFeeException)
            {
                return NotFound(examFeeValidationException.InnerException);
            }
            catch (ExamFeeValidationException examFeeValidationException)
            {
                return BadRequest(examFeeValidationException.InnerException);
            }
            catch (ExamFeeDependencyException examFeeDependencyException)
               when (examFeeDependencyException.InnerException is LockedExamFeeException)
            {
                return Locked(examFeeDependencyException.InnerException);
            }
            catch (ExamFeeDependencyException examFeeDependencyException)
            {
                return InternalServerError(examFeeDependencyException);
            }
            catch (ExamFeeServiceException examFeeServiceException)
            {
                return InternalServerError(examFeeServiceException);
            }
        }
    }
}