// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Guardians;
using OtripleS.Web.Api.Models.Guardians.Exceptions;
using OtripleS.Web.Api.Services.Guardians;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuardiansController : RESTFulController
    {
        private readonly IGuardianService guardianService;

        public GuardiansController(IGuardianService guardianService) =>
            this.guardianService = guardianService;

        [HttpPost]
        public async ValueTask<ActionResult<Guardian>> PostGuardianAsync(Guardian guardian)
        {
            try
            {
                Guardian persistedGuardian =
                    await this.guardianService.CreateGuardianAsync(guardian);

                return Ok(persistedGuardian);
            }
            catch (GuardianValidationException guardianValidationException)
                when (guardianValidationException.InnerException is AlreadyExistsGuardianException)
            {
                string innerMessage = GetInnerMessage(guardianValidationException);

                return Conflict(innerMessage);
            }
            catch (GuardianValidationException guardianValidationException)
            {
                string innerMessage = GetInnerMessage(guardianValidationException);

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
                IQueryable storageGuardian =
                    this.guardianService.RetrieveAllGuardians();

                return Ok(storageGuardian);
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
        public async ValueTask<ActionResult<Guardian>> GetGuardianAsync(Guid guardianId)
        {
            try
            {
                Guardian storageGuardian =
                    await this.guardianService.RetrieveGuardianByIdAsync(guardianId);

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
                Guardian storageGuardian =
                    await this.guardianService.ModifyGuardianAsync(guardian);

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

                return BadRequest(innerMessage);
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

        [HttpDelete("{guardianId}")]
        public async ValueTask<ActionResult<Guardian>> DeleteGuardianAsync(Guid guardianId)
        {
            try
            {
                Guardian storageGuardian =
                    await this.guardianService.DeleteGuardianByIdAsync(guardianId);

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

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
