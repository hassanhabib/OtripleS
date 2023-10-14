// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.GuardianContacts;
using OtripleS.Web.Api.Models.GuardianContacts.Exceptions;
using OtripleS.Web.Api.Services.Foundations.GuardianContacts;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuardianContactsController : RESTFulController
    {
        private readonly IGuardianContactService guardianContactService;

        public GuardianContactsController(IGuardianContactService guardianContactService) =>
            this.guardianContactService = guardianContactService;

        [HttpPost]
        public async ValueTask<ActionResult<GuardianContact>> PostGuardianContactAsync(
            GuardianContact guardianContact)
        {
            try
            {
                GuardianContact addedGuardianContact =
                    await this.guardianContactService.AddGuardianContactAsync(guardianContact);

                return Created(addedGuardianContact);
            }
            catch (GuardianContactValidationException guardianContactValidationException)
                when (guardianContactValidationException.InnerException is AlreadyExistsGuardianContactException)
            {
                return Conflict(guardianContactValidationException.GetInnerMessage());
            }
            catch (GuardianContactValidationException guardianContactValidationException)
            {
                return BadRequest(guardianContactValidationException.GetInnerMessage());
            }
            catch (GuardianContactDependencyException guardianContactDependencyException)
            {
                return Problem(guardianContactDependencyException.Message);
            }
            catch (GuardianContactServiceException guardianContactServiceException)
            {
                return Problem(guardianContactServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<GuardianContact>> GetAllGuardianContacts()
        {
            try
            {
                IQueryable storageGuardianContacts =
                    this.guardianContactService.RetrieveAllGuardianContacts();

                return Ok(storageGuardianContacts);
            }
            catch (GuardianContactDependencyException guardianContactDependencyException)
            {
                return Problem(guardianContactDependencyException.Message);
            }
            catch (GuardianContactServiceException guardianContactServiceException)
            {
                return Problem(guardianContactServiceException.Message);
            }
        }

        [HttpGet("guardians/{guardianId}/contacts/{contactId}")]
        public async ValueTask<ActionResult<GuardianContact>> GetGuardianContactAsync(Guid guardianId, Guid contactId)
        {
            try
            {
                GuardianContact storageGuardianContact =
                    await this.guardianContactService.RetrieveGuardianContactByIdAsync(guardianId, contactId);

                return Ok(storageGuardianContact);
            }
            catch (GuardianContactValidationException semesterGuardianContactValidationException)
                when (semesterGuardianContactValidationException.InnerException is NotFoundGuardianContactException)
            {
                return NotFound(semesterGuardianContactValidationException.GetInnerMessage());
            }
            catch (GuardianContactValidationException semesterGuardianContactValidationException)
            {
                return BadRequest(semesterGuardianContactValidationException.GetInnerMessage());
            }
            catch (GuardianContactDependencyException semesterGuardianContactDependencyException)
            {
                return Problem(semesterGuardianContactDependencyException.Message);
            }
            catch (GuardianContactServiceException semesterGuardianContactServiceException)
            {
                return Problem(semesterGuardianContactServiceException.Message);
            }
        }

        [HttpDelete("guardians/{guardianId}/contacts/{contactId}")]
        public async ValueTask<ActionResult<bool>> DeleteGuardianContactAsync(Guid guardianId, Guid contactId)
        {
            try
            {
                GuardianContact deletedGuardianContact =
                    await this.guardianContactService.RemoveGuardianContactByIdAsync(guardianId, contactId);

                return Ok(deletedGuardianContact);
            }
            catch (GuardianContactValidationException guardianContactValidationException)
                when (guardianContactValidationException.InnerException is NotFoundGuardianContactException)
            {
                return NotFound(guardianContactValidationException.GetInnerMessage());
            }
            catch (GuardianContactValidationException guardianContactValidationException)
            {
                return BadRequest(guardianContactValidationException.GetInnerMessage());
            }
            catch (GuardianContactDependencyException guardianContactValidationException)
               when (guardianContactValidationException.InnerException is LockedGuardianContactException)
            {
                return Locked(guardianContactValidationException.GetInnerMessage());
            }
            catch (GuardianContactDependencyException guardianContactDependencyException)
            {
                return Problem(guardianContactDependencyException.Message);
            }
            catch (GuardianContactServiceException guardianContactServiceException)
            {
                return Problem(guardianContactServiceException.Message);
            }
        }

    }
}
