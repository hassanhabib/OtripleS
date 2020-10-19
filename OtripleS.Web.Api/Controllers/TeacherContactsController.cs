// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;
using OtripleS.Web.Api.Services.TeacherContacts;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherContactsController : RESTFulController
    {
        private readonly ITeacherContactService teacherContactService;

        public TeacherContactsController(ITeacherContactService teacherContactService) =>
            this.teacherContactService = teacherContactService;

        [HttpPost]
        public async ValueTask<ActionResult<TeacherContact>> PostTeacherContactAsync(
            TeacherContact teacherContact)
        {
            try
            {
                TeacherContact persistedTeacherContact =
                    await this.teacherContactService.AddTeacherContactAsync(teacherContact);

                return Ok(persistedTeacherContact);
            }
            catch (TeacherContactValidationException teacherContactValidationException)
                when (teacherContactValidationException.InnerException is AlreadyExistsTeacherContactException)
            {
                string innerMessage = GetInnerMessage(teacherContactValidationException);

                return Conflict(innerMessage);
            }
            catch (TeacherContactValidationException teacherContactValidationException)
            {
                string innerMessage = GetInnerMessage(teacherContactValidationException);

                return BadRequest(innerMessage);
            }
            catch (TeacherContactDependencyException teacherContactDependencyException)
            {
                return Problem(teacherContactDependencyException.Message);
            }
            catch (TeacherContactServiceException teacherContactServiceException)
            {
                return Problem(teacherContactServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<TeacherContact>> GetAllTeacherContacts()
        {
            try
            {
                IQueryable storageTeacherContacts =
                    this.teacherContactService.RetrieveAllTeacherContacts();

                return Ok(storageTeacherContacts);
            }
            catch (TeacherContactDependencyException teacherContactDependencyException)
            {
                return Problem(teacherContactDependencyException.Message);
            }
            catch (TeacherContactServiceException teacherContactServiceException)
            {
                return Problem(teacherContactServiceException.Message);
            }
        }

        [HttpGet("teachers/{teacherId}/contacts/{contactId}")]
        public async ValueTask<ActionResult<TeacherContact>> GetTeacherContactAsync(Guid teacherId, Guid contactId)
        {
            try
            {
                TeacherContact storageTeacherContact =
                    await this.teacherContactService.RetrieveTeacherContactByIdAsync(teacherId, contactId);

                return Ok(storageTeacherContact);
            }
            catch (TeacherContactValidationException semesterTeacherContactValidationException)
                when (semesterTeacherContactValidationException.InnerException is NotFoundTeacherContactException)
            {
                string innerMessage = GetInnerMessage(semesterTeacherContactValidationException);

                return NotFound(innerMessage);
            }
            catch (TeacherContactValidationException semesterTeacherContactValidationException)
            {
                string innerMessage = GetInnerMessage(semesterTeacherContactValidationException);

                return BadRequest(innerMessage);
            }
            catch (TeacherContactDependencyException semesterTeacherContactDependencyException)
            {
                return Problem(semesterTeacherContactDependencyException.Message);
            }
            catch (TeacherContactServiceException semesterTeacherContactServiceException)
            {
                return Problem(semesterTeacherContactServiceException.Message);
            }
        }

        [HttpDelete("teachers/{teacherId}/contacts/{contactId}")]
        public async ValueTask<ActionResult<bool>> DeleteTeacherContactAsync(Guid teacherId, Guid contactId)
        {
            try
            {
                TeacherContact deletedTeacherContact =
                    await this.teacherContactService.RemoveTeacherContactByIdAsync(teacherId, contactId);

                return Ok(deletedTeacherContact);
            }
            catch (TeacherContactValidationException teacherContactValidationException)
                when (teacherContactValidationException.InnerException is NotFoundTeacherContactException)
            {
                string innerMessage = GetInnerMessage(teacherContactValidationException);

                return NotFound(innerMessage);
            }
            catch (TeacherContactValidationException teacherContactValidationException)
            {
                string innerMessage = GetInnerMessage(teacherContactValidationException);

                return BadRequest(innerMessage);
            }
            catch (TeacherContactDependencyException teacherContactValidationException)
               when (teacherContactValidationException.InnerException is LockedTeacherContactException)
            {
                string innerMessage = GetInnerMessage(teacherContactValidationException);

                return Locked(innerMessage);
            }
            catch (TeacherContactDependencyException teacherContactDependencyException)
            {
                return Problem(teacherContactDependencyException.Message);
            }
            catch (TeacherContactServiceException teacherContactServiceException)
            {
                return Problem(teacherContactServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
