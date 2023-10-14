// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------


using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.Contacts.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Contacts;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : RESTFulController
    {
        private readonly IContactService contactService;

        public ContactsController(IContactService contactService) => this.contactService = contactService;

        [HttpPost]
        public async ValueTask<ActionResult<Contact>> PostContactAsync(Contact contact)
        {
            try
            {
                Contact addedContact = await contactService.AddContactAsync(contact);

                return Created(addedContact);
            }
            catch (ContactValidationException contactValidationException)
                when (contactValidationException.InnerException is AlreadyExistsContactException)
            {
                return Conflict(contactValidationException.GetInnerMessage());
            }
            catch (ContactValidationException contactValidationException)
            {
                return BadRequest(contactValidationException.InnerException);
            }
            catch (ContactDependencyException contactDependencyException)
            {
                return Problem(contactDependencyException.GetInnerMessage());
            }
            catch (ContactServiceException contactServiceException)
            {
                return Problem(contactServiceException.GetInnerMessage());
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Contact>> GetAllContacts()
        {
            try
            {
                IQueryable<Contact> storageContacts = contactService.RetrieveAllContacts();

                return Ok(storageContacts);
            }
            catch (ContactDependencyException contactDependencyException)
            {
                return Problem(contactDependencyException.GetInnerMessage());
            }
            catch (ContactServiceException contactServiceException)
            {
                return Problem(contactServiceException.GetInnerMessage());
            }
        }

        [HttpGet("{contactId}")]
        public async ValueTask<ActionResult<Contact>> GetContactByIdAsync(Guid contactId)
        {
            try
            {
                Contact storageContact = await this.contactService.RetrieveContactByIdAsync(contactId);

                return Ok(storageContact);
            }
            catch (ContactValidationException contactValidationException)
                when (contactValidationException.InnerException is NotFoundContactException)
            {
                return NotFound(contactValidationException.GetInnerMessage());
            }
            catch (ContactValidationException contactValidationException)
            {
                return BadRequest(contactValidationException.GetInnerMessage());
            }
            catch (ContactDependencyException contactDependencyException)
            {
                return Problem(contactDependencyException.GetInnerMessage());
            }
            catch (ContactServiceException contactServiceException)
            {
                return Problem(contactServiceException.GetInnerMessage());
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Contact>> PutContactAsync(Contact contact)
        {
            try
            {
                Contact storageContact = await this.contactService.ModifyContactAsync(contact);

                return Ok(storageContact);
            }
            catch (ContactValidationException contactValidationException)
               when (contactValidationException.InnerException is NotFoundContactException)
            {
                return NotFound(contactValidationException.GetInnerMessage());
            }

            catch (ContactValidationException contactValidationException)
            {
                return NotFound(contactValidationException.GetInnerMessage());
            }

            catch (ContactDependencyException contactDependencyException)
                when (contactDependencyException.InnerException is LockedContactException)
            {
                return NotFound(contactDependencyException.GetInnerMessage());
            }
        }

        [HttpDelete("{contactId}")]
        public async ValueTask<ActionResult<Contact>> DeleteContactAsync(Guid contactId)
        {
            try
            {
                Contact storageContact =
                    await this.contactService.RemoveContactByIdAsync(contactId);

                return Ok(storageContact);
            }
            catch (ContactValidationException contactValidationException)
                when (contactValidationException.InnerException is NotFoundContactException)
            {
                return NotFound(contactValidationException.GetInnerMessage());
            }
            catch (ContactValidationException contactValidationException)
            {
                return BadRequest(contactValidationException);
            }
            catch (ContactDependencyException contactDependencyException)
               when (contactDependencyException.InnerException is LockedContactException)
            {
                return Locked(contactDependencyException.GetInnerMessage());
            }
            catch (ContactDependencyException contactDependencyException)
            {
                return Problem(contactDependencyException.Message);
            }
            catch (ContactServiceException contactServiceException)
            {
                return Problem(contactServiceException.Message);
            }
        }

    }
}
