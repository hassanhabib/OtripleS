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

                return Created(persistedUserContact);
            }
            catch (UserContactValidationException userContactValidationException)
                when (userContactValidationException.InnerException is AlreadyExistsUserContactException)
            {
                return Conflict(userContactValidationException);
            }
            catch (UserContactValidationException userContactValidationException)
            {
                return BadRequest(userContactValidationException);
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
                return NotFound(semesterUserContactValidationException);
            }
            catch (UserContactValidationException semesterUserContactValidationException)
            {
                return BadRequest(semesterUserContactValidationException);
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
                return NotFound(userContactValidationException);
            }
            catch (UserContactValidationException userContactValidationException)
            {
                return BadRequest(userContactValidationException);
            }
            catch (UserContactDependencyException userContactValidationException)
               when (userContactValidationException.InnerException is LockedUserContactException)
            {
                return Locked(userContactValidationException);
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
