// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
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
    }
}
