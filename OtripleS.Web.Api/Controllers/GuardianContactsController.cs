// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.GuardianContacts;
using OtripleS.Web.Api.Models.GuardianContacts.Exceptions;
using OtripleS.Web.Api.Services.GuardianContacts;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                GuardianContact persistedGuardianContact =
                    await this.guardianContactService.AddGuardianContactAsync(guardianContact);

                return Ok(persistedGuardianContact);
            }
            catch (GuardianContactValidationException guardianContactValidationException)
                when (guardianContactValidationException.InnerException is AlreadyExistsGuardianContactException)
            {
                string innerMessage = GetInnerMessage(guardianContactValidationException);

                return Conflict(innerMessage);
            }
            catch (GuardianContactValidationException guardianContactValidationException)
            {
                string innerMessage = GetInnerMessage(guardianContactValidationException);

                return BadRequest(innerMessage);
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
                string innerMessage = GetInnerMessage(semesterGuardianContactValidationException);

                return NotFound(innerMessage);
            }
            catch (GuardianContactValidationException semesterGuardianContactValidationException)
            {
                string innerMessage = GetInnerMessage(semesterGuardianContactValidationException);

                return BadRequest(innerMessage);
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
                string innerMessage = GetInnerMessage(guardianContactValidationException);

                return NotFound(innerMessage);
            }
            catch (GuardianContactValidationException guardianContactValidationException)
            {
                string innerMessage = GetInnerMessage(guardianContactValidationException);

                return BadRequest(innerMessage);
            }
            catch (GuardianContactDependencyException guardianContactValidationException)
               when (guardianContactValidationException.InnerException is LockedGuardianContactException)
            {
                string innerMessage = GetInnerMessage(guardianContactValidationException);

                return Locked(innerMessage);
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

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
