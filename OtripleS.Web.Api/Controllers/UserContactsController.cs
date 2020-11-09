// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;
using OtripleS.Web.Api.Services.UserContacts;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserContactsController : RESTFulController
    {
        private readonly IUserContactService userContactService;

        public UserContactsController(IUserContactService userContactService) =>
            this.userContactService = userContactService;

        [HttpPost]
        public async ValueTask<ActionResult<UserContact>> PostUserContactAsync(
            UserContact userContact)
        {
            try
            {
                UserContact persistedUserContact =
                    await this.userContactService.AddUserContactAsync(userContact);

                return Ok(persistedUserContact);
            }
            catch (UserContactValidationException userContactValidationException)
                when (userContactValidationException.InnerException is AlreadyExistsUserContactException)
            {
                string innerMessage = GetInnerMessage(userContactValidationException);

                return Conflict(innerMessage);
            }
            catch (UserContactValidationException userContactValidationException)
            {
                string innerMessage = GetInnerMessage(userContactValidationException);

                return BadRequest(innerMessage);
            }
            catch (UserContactDependencyException userContactDependencyException)
            {
                return Problem(userContactDependencyException.Message);
            }
            catch (UserContactServiceException userContactServiceException)
            {
                return Problem(userContactServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<UserContact>> GetAllUserContacts()
        {
            try
            {
                IQueryable storageUserContacts =
                    this.userContactService.RetrieveAllUserContacts();

                return Ok(storageUserContacts);
            }
            catch (UserContactDependencyException userContactDependencyException)
            {
                return Problem(userContactDependencyException.Message);
            }
            catch (UserContactServiceException userContactServiceException)
            {
                return Problem(userContactServiceException.Message);
            }
        }

        [HttpGet("users/{userId}/contacts/{contactId}")]
        public async ValueTask<ActionResult<UserContact>> GetUserContactAsync(Guid userId, Guid contactId)
        {
            try
            {
                UserContact storageUserContact =
                    await this.userContactService.RetrieveUserContactByIdAsync(userId, contactId);

                return Ok(storageUserContact);
            }
            catch (UserContactValidationException semesterUserContactValidationException)
                when (semesterUserContactValidationException.InnerException is NotFoundUserContactException)
            {
                string innerMessage = GetInnerMessage(semesterUserContactValidationException);

                return NotFound(innerMessage);
            }
            catch (UserContactValidationException semesterUserContactValidationException)
            {
                string innerMessage = GetInnerMessage(semesterUserContactValidationException);

                return BadRequest(innerMessage);
            }
            catch (UserContactDependencyException semesterUserContactDependencyException)
            {
                return Problem(semesterUserContactDependencyException.Message);
            }
            catch (UserContactServiceException semesterUserContactServiceException)
            {
                return Problem(semesterUserContactServiceException.Message);
            }
        }

        [HttpDelete("users/{userId}/contacts/{contactId}")]
        public async ValueTask<ActionResult<bool>> DeleteUserContactAsync(Guid userId, Guid contactId)
        {
            try
            {
                UserContact deletedUserContact =
                    await this.userContactService.RemoveUserContactByIdAsync(userId, contactId);

                return Ok(deletedUserContact);
            }
            catch (UserContactValidationException userContactValidationException)
                when (userContactValidationException.InnerException is NotFoundUserContactException)
            {
                string innerMessage = GetInnerMessage(userContactValidationException);

                return NotFound(innerMessage);
            }
            catch (UserContactValidationException userContactValidationException)
            {
                string innerMessage = GetInnerMessage(userContactValidationException);

                return BadRequest(innerMessage);
            }
            catch (UserContactDependencyException userContactValidationException)
               when (userContactValidationException.InnerException is LockedUserContactException)
            {
                string innerMessage = GetInnerMessage(userContactValidationException);

                return Locked(innerMessage);
            }
            catch (UserContactDependencyException userContactDependencyException)
            {
                return Problem(userContactDependencyException.Message);
            }
            catch (UserContactServiceException userContactServiceException)
            {
                return Problem(userContactServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
