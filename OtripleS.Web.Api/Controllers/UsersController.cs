// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Users;
using OtripleS.Web.Api.Models.Users.Exceptions;
using OtripleS.Web.Api.Services.Users;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : RESTFulController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService) =>
            this.userService = userService;

        [HttpGet]
        public ActionResult<IQueryable<User>> GetAllUsers()
        {
            try
            {
                IQueryable storageUsers =
                    this.userService.RetrieveAllUsers();

                return Ok(storageUsers);
            }
            catch (UserDependencyException userDependencyException)
            {
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }

        [HttpGet("{userId}")]
        public async ValueTask<ActionResult<User>> GetUserByIdAsync(Guid userId)
        {
            try
            {
                User storageUser =
                    await this.userService.RetrieveUserByIdAsync(userId);

                return Ok(storageUser);
            }
            catch (UserValidationException userValidationException)
                when (userValidationException.InnerException is NotFoundUserException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return NotFound(innerMessage);
            }
            catch (UserValidationException userValidationException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return BadRequest(innerMessage);
            }
            catch (UserDependencyException userDependencyException)
            {
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }

        [HttpPost]
        public async ValueTask<ActionResult<User>> PostUserAsync(User user, string password = "Test123@eri")
        {
            try
            {
                User persistedUser =
                    await this.userService.RegisterUserAsync(user, password);

                return Ok(persistedUser);
            }
            catch (UserValidationException userValidationException)
                when (userValidationException.InnerException is AlreadyExistsUserException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return Conflict(innerMessage);
            }
            catch (UserValidationException userValidationException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return BadRequest(innerMessage);
            }
            catch (UserDependencyException userDependencyException)
            {
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<User>> PutUserAsync(User user)
        {
            try
            {
                User registeredUser =
                    await this.userService.ModifyUserAsync(user);

                return Ok(registeredUser);
            }
            catch (UserValidationException userValidationException)
                when (userValidationException.InnerException is NotFoundUserException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return NotFound(innerMessage);
            }
            catch (UserValidationException userValidationException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return BadRequest(innerMessage);
            }
            catch (UserDependencyException userDependencyException)
                when (userDependencyException.InnerException is LockedUserException)
            {
                string innerMessage = GetInnerMessage(userDependencyException);

                return Locked(innerMessage);
            }
            catch (UserDependencyException userDependencyException)
            {
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async ValueTask<ActionResult<User>> DeleteUserAsync(Guid userId)
        {
            try
            {
                User storageUser =
                    await this.userService.DeleteUserAsync(userId);

                return Ok(storageUser);
            }
            catch (UserValidationException userValidationException)
                when (userValidationException.InnerException is NotFoundUserException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return NotFound(innerMessage);
            }
            catch (UserValidationException userValidationException)
            {
                return BadRequest(userValidationException.Message);
            }
            catch (UserDependencyException userDependencyException)
                when (userDependencyException.InnerException is LockedUserException)
            {
                string innerMessage = GetInnerMessage(userDependencyException);

                return Locked(innerMessage);
            }
            catch (UserDependencyException userDependencyException)
            {
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
