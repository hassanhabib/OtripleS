﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.Guardians;
using OtripleS.Web.Api.Models.Guardians.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Guardians;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                Guardian createdGuardian =
                    await this.guardianService.CreateGuardianAsync(guardian);

                return Created(createdGuardian);
            }
            catch (GuardianValidationException guardianValidationException)
                when (guardianValidationException.InnerException is AlreadyExistsGuardianException)
            {
                return Conflict(guardianValidationException.GetInnerMessage());
            }
            catch (GuardianValidationException guardianValidationException)
            {
                return BadRequest(guardianValidationException.GetInnerMessage());
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
                return NotFound(guardianValidationException.GetInnerMessage());
            }
            catch (GuardianValidationException guardianValidationException)
            {
                return BadRequest(guardianValidationException.GetInnerMessage());
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
                return NotFound(guardianValidationException.GetInnerMessage());
            }
            catch (GuardianValidationException guardianValidationException)
            {
                return BadRequest(guardianValidationException.GetInnerMessage());
            }
            catch (GuardianDependencyException guardianDependencyException)
                when (guardianDependencyException.InnerException is LockedGuardianException)
            {
                return Locked(guardianDependencyException.GetInnerMessage());
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
                    await this.guardianService.RemoveGuardianByIdAsync(guardianId);

                return Ok(storageGuardian);
            }
            catch (GuardianValidationException guardianValidationException)
                when (guardianValidationException.InnerException is NotFoundGuardianException)
            {
                return NotFound(guardianValidationException.GetInnerMessage());
            }
            catch (GuardianValidationException guardianValidationException)
            {
                return BadRequest(guardianValidationException.GetInnerMessage());
            }
            catch (GuardianDependencyException guardianDependencyException)
               when (guardianDependencyException.InnerException is LockedGuardianException)
            {
                return Locked(guardianDependencyException.GetInnerMessage());
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

    }
}
