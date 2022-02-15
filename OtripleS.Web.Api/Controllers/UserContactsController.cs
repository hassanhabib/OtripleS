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
using OtripleS.Web.Api.Services.Foundations.UserContacts;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                UserContact addedUserContact =
                    await this.userContactService.AddUserContactAsync(userContact);

                return Created(addedUserContact);
            }
            catch (UserContactValidationException userContactValidationException)
                when (userContactValidationException.InnerException is AlreadyExistsUserContactException)
            {
                return Conflict(userContactValidationException.InnerException);
            }
            catch (UserContactValidationException userContactValidationException)
            {
                return BadRequest(userContactValidationException.InnerException);
            }
            catch (UserContactDependencyException userContactDependencyException)
            {
                return InternalServerError(userContactDependencyException);
            }
            catch (UserContactServiceException userContactServiceException)
            {
                return InternalServerError(userContactServiceException);
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
                return InternalServerError(userContactDependencyException);
            }
            catch (UserContactServiceException userContactServiceException)
            {
                return InternalServerError(userContactServiceException);
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
                return NotFound(semesterUserContactValidationException.InnerException);
            }
            catch (UserContactValidationException semesterUserContactValidationException)
            {
                return BadRequest(semesterUserContactValidationException.InnerException);
            }
            catch (UserContactDependencyException semesterUserContactDependencyException)
            {
                return InternalServerError(semesterUserContactDependencyException);
            }
            catch (UserContactServiceException semesterUserContactServiceException)
            {
                return InternalServerError(semesterUserContactServiceException);
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
                return NotFound(userContactValidationException.InnerException);
            }
            catch (UserContactValidationException userContactValidationException)
            {
                return BadRequest(userContactValidationException.InnerException);
            }
            catch (UserContactDependencyException userContactValidationException)
               when (userContactValidationException.InnerException is LockedUserContactException)
            {
                return Locked(userContactValidationException.InnerException);
            }
            catch (UserContactDependencyException userContactDependencyException)
            {
                return InternalServerError(userContactDependencyException);
            }
            catch (UserContactServiceException userContactServiceException)
            {
                return InternalServerError(userContactServiceException);
            }
        }
    }
}
