// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Fees;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                Fee addedFee =
                    await this.feeService.AddFeeAsync(fee);

                return Created(addedFee);
            }
            catch (FeeValidationException feeValidationException)
                when (feeValidationException.InnerException is AlreadyExistsFeeException)
            {
                return Conflict(feeValidationException.GetInnerMessage());
            }
            catch (FeeValidationException feeValidationException)
            {
                return BadRequest(feeValidationException.GetInnerMessage());
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
                return NotFound(feeValidationException.GetInnerMessage());
            }
            catch (FeeValidationException feeValidationException)
            {
                return BadRequest(feeValidationException.GetInnerMessage());
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
                return NotFound(feeValidationException.GetInnerMessage());
            }
            catch (FeeValidationException feeValidationException)
            {
                return BadRequest(feeValidationException.GetInnerMessage());
            }
            catch (FeeDependencyException feeDependencyException)
                when (feeDependencyException.InnerException is LockedFeeException)
            {
                return Locked(feeDependencyException.GetInnerMessage());
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
                    await this.feeService.RemoveFeeByIdAsync(feeId);

                return Ok(storageFee);
            }
            catch (FeeValidationException feeValidationException)
                when (feeValidationException.InnerException is NotFoundFeeException)
            {
                return NotFound(feeValidationException.GetInnerMessage());
            }
            catch (FeeValidationException feeValidationException)
            {
                return BadRequest(feeValidationException);
            }
            catch (FeeDependencyException feeDependencyException)
               when (feeDependencyException.InnerException is LockedFeeException)
            {
                return Locked(feeDependencyException.GetInnerMessage());
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

    }
}