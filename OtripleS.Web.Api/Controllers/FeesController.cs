// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;
using OtripleS.Web.Api.Services.Fees;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeesController : RESTFulController
    {
        private readonly IFeeService feeService;

        public FeesController(IFeeService feeService) =>
            this.feeService = feeService;

        [HttpPost]
        public async ValueTask<ActionResult<Fee>> PostFeeAsync(Fee fee)
        {
            try
            {
                Fee persistedFee =
                    await this.feeService.CreateFeeAsync(fee);

                return Ok(persistedFee);
            }
            catch (FeeValidationException feeValidationException)
                when (feeValidationException.InnerException is AlreadyExistsFeeException)
            {
                string innerMessage = GetInnerMessage(feeValidationException);

                return Conflict(innerMessage);
            }
            catch (FeeValidationException feeValidationException)
            {
                string innerMessage = GetInnerMessage(feeValidationException);

                return BadRequest(innerMessage);
            }
            catch (FeeDependencyException feeDependencyException)
            {
                return Problem(feeDependencyException.Message);
            }
            catch (FeeServiceException feeServiceException)
            {
                return Problem(feeServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Fee>> GetAllFees()
        {
            try
            {
                IQueryable storageFee =
                    this.feeService.RetrieveAllFees();

                return Ok(storageFee);
            }
            catch (FeeDependencyException feeDependencyException)
            {
                return Problem(feeDependencyException.Message);
            }
            catch (FeeServiceException feeServiceException)
            {
                return Problem(feeServiceException.Message);
            }
        }

        [HttpGet("{feeId}")]
        public async ValueTask<ActionResult<Fee>> GetFeeAsync(Guid feeId)
        {
            try
            {
                Fee storageFee =
                    await this.feeService.RetrieveFeeByIdAsync(feeId);

                return Ok(storageFee);
            }
            catch (FeeValidationException feeValidationException)
                when (feeValidationException.InnerException is NotFoundFeeException)
            {
                string innerMessage = GetInnerMessage(feeValidationException);

                return NotFound(innerMessage);
            }
            catch (FeeValidationException feeValidationException)
            {
                string innerMessage = GetInnerMessage(feeValidationException);

                return BadRequest(innerMessage);
            }
            catch (FeeDependencyException feeDependencyException)
            {
                return Problem(feeDependencyException.Message);
            }
            catch (FeeServiceException feeServiceException)
            {
                return Problem(feeServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Fee>> PutFeeAsync(Fee fee)
        {
            try
            {
                Fee registeredFee =
                    await this.feeService.ModifyFeeAsync(fee);

                return Ok(registeredFee);
            }
            catch (FeeValidationException feeValidationException)
                when (feeValidationException.InnerException is NotFoundFeeException)
            {
                string innerMessage = GetInnerMessage(feeValidationException);

                return NotFound(innerMessage);
            }
            catch (FeeValidationException feeValidationException)
            {
                string innerMessage = GetInnerMessage(feeValidationException);

                return BadRequest(innerMessage);
            }
            catch (FeeDependencyException feeDependencyException)
                when (feeDependencyException.InnerException is LockedFeeException)
            {
                string innerMessage = GetInnerMessage(feeDependencyException);

                return Locked(innerMessage);
            }
            catch (FeeDependencyException feeDependencyException)
            {
                return Problem(feeDependencyException.Message);
            }
            catch (FeeServiceException feeServiceException)
            {
                return Problem(feeServiceException.Message);
            }
        }

        [HttpDelete("{feeId}")]
        public async ValueTask<ActionResult<Fee>> DeleteFeeAsync(Guid feeId)
        {
            try
            {
                Fee storageFee =
                    await this.feeService.RemoveFeeAsync(feeId);

                return Ok(storageFee);
            }
            catch (FeeValidationException feeValidationException)
                when (feeValidationException.InnerException is NotFoundFeeException)
            {
                string innerMessage = GetInnerMessage(feeValidationException);

                return NotFound(innerMessage);
            }
            catch (FeeValidationException feeValidationException)
            {
                string innerMessage = GetInnerMessage(feeValidationException);

                return BadRequest(feeValidationException);
            }
            catch (FeeDependencyException feeDependencyException)
               when (feeDependencyException.InnerException is LockedFeeException)
            {
                string innerMessage = GetInnerMessage(feeDependencyException);

                return Locked(innerMessage);
            }
            catch (FeeDependencyException feeDependencyException)
            {
                return Problem(feeDependencyException.Message);
            }
            catch (FeeServiceException feeServiceException)
            {
                return Problem(feeServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}