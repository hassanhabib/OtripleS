// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Registrations;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : RESTFulController
    {
        private readonly IRegistrationService registrationService;

        public RegistrationsController(IRegistrationService registrationService) =>
            this.registrationService = registrationService;

        [HttpPost]
        public async ValueTask<ActionResult<Registration>> PostRegistrationAsync(Registration registration)
        {
            try
            {
                Registration addedRegistration =
                    await this.registrationService.AddRegistrationAsync(registration);

                return Created(addedRegistration);
            }
            catch (RegistrationValidationException registrationValidationException)
                when (registrationValidationException.InnerException is AlreadyExistsRegistrationException)
            {
                return Conflict(registrationValidationException.GetInnerMessage());
            }
            catch (RegistrationValidationException registrationValidationException)
            {
                return BadRequest(registrationValidationException.GetInnerMessage());
            }
            catch (RegistrationDependencyException registrationDependencyException)
            {
                return Problem(registrationDependencyException.Message);
            }
            catch (RegistrationServiceException registrationServiceException)
            {
                return Problem(registrationServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Registration>> GetAllRegistrations()
        {
            try
            {
                IQueryable storageRegistration =
                    this.registrationService.RetrieveAllRegistrations();

                return Ok(storageRegistration);
            }
            catch (RegistrationDependencyException registrationDependencyException)
            {
                return Problem(registrationDependencyException.Message);
            }
            catch (RegistrationServiceException registrationServiceException)
            {
                return Problem(registrationServiceException.Message);
            }
        }

        [HttpGet("{registrationId}")]
        public async ValueTask<ActionResult<Registration>> GetRegistrationAsync(Guid registrationId)
        {
            try
            {
                Registration storageRegistration =
                    await this.registrationService.RetrieveRegistrationByIdAsync(registrationId);

                return Ok(storageRegistration);
            }
            catch (RegistrationValidationException registrationValidationException)
                when (registrationValidationException.InnerException is NotFoundRegistrationException)
            {
                return NotFound(registrationValidationException.GetInnerMessage());
            }
            catch (RegistrationValidationException registrationValidationException)
            {
                return BadRequest(registrationValidationException.GetInnerMessage());
            }
            catch (RegistrationDependencyException registrationDependencyException)
            {
                return Problem(registrationDependencyException.Message);
            }
            catch (RegistrationServiceException registrationServiceException)
            {
                return Problem(registrationServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Registration>> PutRegistrationAsync(Registration registration)
        {
            try
            {
                Registration registeredRegistration =
                    await this.registrationService.ModifyRegistrationAsync(registration);

                return Ok(registeredRegistration);
            }
            catch (RegistrationValidationException registrationValidationException)
                when (registrationValidationException.InnerException is NotFoundRegistrationException)
            {
                return NotFound(registrationValidationException.GetInnerMessage());
            }
            catch (RegistrationValidationException registrationValidationException)
            {
                return BadRequest(registrationValidationException.GetInnerMessage());
            }
            catch (RegistrationDependencyException registrationDependencyException)
                when (registrationDependencyException.InnerException is LockedRegistrationException)
            {
                return Locked(registrationDependencyException.GetInnerMessage());
            }
            catch (RegistrationDependencyException registrationDependencyException)
            {
                return Problem(registrationDependencyException.Message);
            }
            catch (RegistrationServiceException registrationServiceException)
            {
                return Problem(registrationServiceException.Message);
            }
        }

        [HttpDelete("{registrationId}")]
        public async ValueTask<ActionResult<Registration>> DeleteRegistrationAsync(Guid registrationId)
        {
            try
            {
                Registration storageRegistration =
                    await this.registrationService.RemoveRegistrationByIdAsync(registrationId);

                return Ok(storageRegistration);
            }
            catch (RegistrationValidationException registrationValidationException)
                when (registrationValidationException.InnerException is NotFoundRegistrationException)
            {
                return NotFound(registrationValidationException.GetInnerMessage());
            }
            catch (RegistrationValidationException registrationValidationException)
            {
                return BadRequest(registrationValidationException);
            }
            catch (RegistrationDependencyException registrationDependencyException)
               when (registrationDependencyException.InnerException is LockedRegistrationException)
            {
                return Locked(registrationDependencyException.GetInnerMessage());
            }
            catch (RegistrationDependencyException registrationDependencyException)
            {
                return Problem(registrationDependencyException.Message);
            }
            catch (RegistrationServiceException registrationServiceException)
            {
                return Problem(registrationServiceException.Message);
            }
        }

    }
}