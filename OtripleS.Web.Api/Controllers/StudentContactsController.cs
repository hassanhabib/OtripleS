// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.StudentContacts;
using OtripleS.Web.Api.Models.StudentContacts.Exceptions;
using OtripleS.Web.Api.Services.Foundations.StudentContacts;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentContactsController : RESTFulController
    {
        private readonly IStudentContactService studentContactService;

        public StudentContactsController(IStudentContactService studentContactService) =>
            this.studentContactService = studentContactService;

        [HttpPost]
        public async ValueTask<ActionResult<StudentContact>> PostStudentContactAsync(
            StudentContact studentContact)
        {
            try
            {
                StudentContact addedStudentContact =
                    await this.studentContactService.AddStudentContactAsync(studentContact);

                return Created(addedStudentContact);
            }
            catch (StudentContactValidationException studentContactValidationException)
                when (studentContactValidationException.InnerException is AlreadyExistsStudentContactException)
            {
                return Conflict(studentContactValidationException.GetInnerMessage());
            }
            catch (StudentContactValidationException studentContactValidationException)
            {
                return BadRequest(studentContactValidationException.GetInnerMessage());
            }
            catch (StudentContactDependencyException studentContactDependencyException)
            {
                return Problem(studentContactDependencyException.Message);
            }
            catch (StudentContactServiceException studentContactServiceException)
            {
                return Problem(studentContactServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<StudentContact>> GetAllStudentContacts()
        {
            try
            {
                IQueryable storageStudentContacts =
                    this.studentContactService.RetrieveAllStudentContacts();

                return Ok(storageStudentContacts);
            }
            catch (StudentContactDependencyException studentContactDependencyException)
            {
                return Problem(studentContactDependencyException.Message);
            }
            catch (StudentContactServiceException studentContactServiceException)
            {
                return Problem(studentContactServiceException.Message);
            }
        }

        [HttpGet("students/{studentId}/contacts/{contactId}")]
        public async ValueTask<ActionResult<StudentContact>> GetStudentContactAsync(Guid studentId, Guid contactId)
        {
            try
            {
                StudentContact storageStudentContact =
                    await this.studentContactService.RetrieveStudentContactByIdAsync(studentId, contactId);

                return Ok(storageStudentContact);
            }
            catch (StudentContactValidationException semesterStudentContactValidationException)
                when (semesterStudentContactValidationException.InnerException is NotFoundStudentContactException)
            {
                return NotFound(semesterStudentContactValidationException.GetInnerMessage());
            }
            catch (StudentContactValidationException semesterStudentContactValidationException)
            {
                return BadRequest(semesterStudentContactValidationException.GetInnerMessage());
            }
            catch (StudentContactDependencyException semesterStudentContactDependencyException)
            {
                return Problem(semesterStudentContactDependencyException.Message);
            }
            catch (StudentContactServiceException semesterStudentContactServiceException)
            {
                return Problem(semesterStudentContactServiceException.Message);
            }
        }

        [HttpDelete("students/{studentId}/contacts/{contactId}")]
        public async ValueTask<ActionResult<bool>> DeleteStudentContactAsync(Guid studentId, Guid contactId)
        {
            try
            {
                StudentContact deletedStudentContact =
                    await this.studentContactService.RemoveStudentContactByIdAsync(studentId, contactId);

                return Ok(deletedStudentContact);
            }
            catch (StudentContactValidationException studentContactValidationException)
                when (studentContactValidationException.InnerException is NotFoundStudentContactException)
            {
                return NotFound(studentContactValidationException.GetInnerMessage());
            }
            catch (StudentContactValidationException studentContactValidationException)
            {
                return BadRequest(studentContactValidationException.GetInnerMessage());
            }
            catch (StudentContactDependencyException studentContactValidationException)
               when (studentContactValidationException.InnerException is LockedStudentContactException)
            {
                return Locked(studentContactValidationException.GetInnerMessage());
            }
            catch (StudentContactDependencyException studentContactDependencyException)
            {
                return Problem(studentContactDependencyException.Message);
            }
            catch (StudentContactServiceException studentContactServiceException)
            {
                return Problem(studentContactServiceException.Message);
            }
        }

    }
}
