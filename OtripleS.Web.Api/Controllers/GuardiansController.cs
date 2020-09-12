// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Guardian;
using OtripleS.Web.Api.Models.Guardian.Exceptions;
using OtripleS.Web.Api.Models.Guardians.Exceptions;
using OtripleS.Web.Api.Services.Guardians;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuardiansController : RESTFulController
    {
        private readonly GuardianService guardianService;

        public GuardiansController(GuardianService guardianService) =>
            this.guardianService = guardianService;

        [HttpPost]
        public async ValueTask<ActionResult<Guardian>> PostGuardianAsync(Guardian guardian)
        {
            try
            {
                Guardian persistedGuardian = await guardianService.CreateGuardianAsync(guardian);

                return Ok(persistedGuardian);
            }
            catch (GuardianValidationException guardianValidationException)
            when (guardianValidationException.InnerException is AlreadyExistsGuardianException)
            {
                string innerMessage = this.GetInnerMessage(guardianValidationException);

                return Conflict(innerMessage);
            }
            catch (GuardianValidationException guardianValidationException)
            {
                string innerMessage = this.GetInnerMessage(guardianValidationException);

                return BadRequest(innerMessage);
            }
            catch (GuardianDependencyException guardianDependencyException)
            {
                return Problem(guardianDependencyException.Message);
            }
            catch (GuardianServiceException guardianServiceException)
            {
                return Problem(guardianServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Guardian>> GetAllGuardians()
        {
            try
            {
                IQueryable storageGuardians = this.guardianService.RetrieveAllGuardians();

                return Ok(storageGuardians);
            }
            catch (GuardianDependencyException guardianDependencyException)
            {
                return Problem(guardianDependencyException.Message);
            }
            catch (GuardianServiceException guardianServiceException)
            {
                return Problem(guardianServiceException.Message);
            }
        }

        [HttpGet("{guardianId}")]
        public async ValueTask<ActionResult<Guardian>> GetGuardianByIdAsync(Guid guardianId)
        {
            try
            {
                Guardian guardian = await this.guardianService.RetrieveGuardianByIdAsync(guardianId);

                return Ok(guardian);
            }
            catch (GuardianValidationException guardianValidationException)
            when (guardianValidationException.InnerException is NotFoundGuardianException)
            {
                string innerMessage = this.GetInnerMessage(guardianValidationException);

                return NotFound(innerMessage);
            }
            catch (GuardianValidationException guardianValidationException)
            {
                string innerMessage = this.GetInnerMessage(guardianValidationException);

                return BadRequest(innerMessage);
            }
            catch (GuardianDependencyException guardianDependencyException)
            {
                return Problem(guardianDependencyException.Message);
            }
            catch (GuardianServiceException guardianServiceException)
            {
                return Problem(guardianServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Guardian>> PutGuardianAsync(Guardian guardian)
        {
            try
            {
                Guardian registeredGuardian = await this.guardianService.ModifyGuardianAsync(guardian);

                return Ok(registeredGuardian);
            }
            catch (GuardianValidationException guardianValidationException)
            when (guardianValidationException.InnerException is NotFoundGuardianException)
            {
                string innerMessage = this.GetInnerMessage(guardianValidationException);

                return NotFound(innerMessage);
            }
            catch (GuardianValidationException guardianValidationException)
            {
                string innerMessage = this.GetInnerMessage(guardianValidationException);

                return BadRequest(innerMessage);
            }
            catch (GuardianDependencyException guardianDependencyException)
            when (guardianDependencyException.InnerException is LockedGuardianException)
            {
                string innerMessage = this.GetInnerMessage(guardianDependencyException);

                return Locked(innerMessage);
            }
            catch (GuardianDependencyException guardianDependencyException)
            {
                return Problem(guardianDependencyException.Message);
            }
            catch (GuardianServiceException guardianServiceException)
            {
                return Problem(guardianServiceException.Message);
            }
        }

        [HttpDelete("{guardianId}")]
        public async ValueTask<ActionResult<Guardian>> DeleteGuardianAsync(Guid guardianId)
        {
            try
            {
                Guardian storageGuardian = await this.guardianService.DeleteGuardianByIdAsync(guardianId);

                return Ok(storageGuardian);
            }
            catch (GuardianValidationException guardianValidationException)
                when (guardianValidationException.InnerException is NotFoundGuardianException)
            {
                string innerMessage = GetInnerMessage(guardianValidationException);

                return NotFound(innerMessage);
            }
            catch (GuardianValidationException guardianValidationException)
            {
                string innerMessage = GetInnerMessage(guardianValidationException);

                return BadRequest(guardianValidationException);
            }
            catch (GuardianDependencyException guardianDependencyException)
               when (guardianDependencyException.InnerException is LockedGuardianException)
            {
                string innerMessage = GetInnerMessage(guardianDependencyException);

                return Locked(innerMessage);
            }
            catch (GuardianDependencyException guardianDependencyException)
            {
                return Problem(guardianDependencyException.Message);
            }
            catch (GuardianServiceException guardianServiceException)
            {
                return Problem(guardianServiceException.Message);
            }
        }
                
        private string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
