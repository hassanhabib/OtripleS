// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Foundations.Registrations;
using OtripleS.Web.Api.Models.Foundations.Registrations.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Registrations;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                Registration persistedRegistration =
                    await this.registrationService.AddRegistrationAsync(registration);

                return Ok(persistedRegistration);
            }
            catch (RegistrationValidationException registrationValidationException)
                when (registrationValidationException.InnerException is AlreadyExistsRegistrationException)
            {
                string innerMessage = GetInnerMessage(registrationValidationException);

                return Conflict(innerMessage);
            }
            catch (RegistrationValidationException registrationValidationException)
            {
                string innerMessage = GetInnerMessage(registrationValidationException);

                return BadRequest(innerMessage);
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
            catch (RegistrationDependencyException RegistrationDependencyException)
            {
                return Problem(RegistrationDependencyException.Message);
            }
            catch (RegistrationServiceException RegistrationServiceException)
            {
                return Problem(RegistrationServiceException.Message);
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
                string innerMessage = GetInnerMessage(registrationValidationException);

                return NotFound(innerMessage);
            }
            catch (RegistrationValidationException registrationValidationException)
            {
                string innerMessage = GetInnerMessage(registrationValidationException);

                return BadRequest(innerMessage);
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
                string innerMessage = GetInnerMessage(registrationValidationException);

                return NotFound(innerMessage);
            }
            catch (RegistrationValidationException registrationValidationException)
            {
                string innerMessage = GetInnerMessage(registrationValidationException);

                return BadRequest(innerMessage);
            }
            catch (RegistrationDependencyException registrationDependencyException)
                when (registrationDependencyException.InnerException is LockedRegistrationException)
            {
                string innerMessage = GetInnerMessage(registrationDependencyException);

                return Locked(innerMessage);
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
                string innerMessage = GetInnerMessage(registrationValidationException);

                return NotFound(innerMessage);
            }
            catch (RegistrationValidationException registrationValidationException)
            {
                string innerMessage = GetInnerMessage(registrationValidationException);

                return BadRequest(registrationValidationException);
            }
            catch (RegistrationDependencyException registrationDependencyException)
               when (registrationDependencyException.InnerException is LockedRegistrationException)
            {
                string innerMessage = GetInnerMessage(registrationDependencyException);

                return Locked(innerMessage);
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

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}