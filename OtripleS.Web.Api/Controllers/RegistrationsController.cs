// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;
using OtripleS.Web.Api.Services.Registrations;
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

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}