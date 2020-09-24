// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.Contacts.Exceptions;
using OtripleS.Web.Api.Services.Contacts;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : RESTFulController
    {
        private readonly IContactService contactService;

        public ContactsController(IContactService contactService) => this.contactService = contactService;

        [HttpPost]
        public async ValueTask<ActionResult<Contact>> PostContactAsync(Contact contact)
        {
            try
            {
                Contact persistedContact = await contactService.AddContactAsync(contact);

                return Ok(persistedContact);
            }
            catch (ContactValidationException contactValidationException)
                when (contactValidationException.InnerException is AlreadyExistsContactException)
            {
                string innerMessage = GetInnerMessage(contactValidationException);

                return Conflict(innerMessage);
            }
            catch (ContactValidationException contactValidationException)
            {
                string innerMessage = GetInnerMessage(contactValidationException);

                return BadRequest(innerMessage);
            }
            catch (ContactDependencyException contactDependencyException)
            {
                string innerMessage = GetInnerMessage(contactDependencyException);

                return Problem(innerMessage);
            }
            catch (ContactServiceException contactServiceException)
            {
                string innerMessage = GetInnerMessage(contactServiceException);

                return Problem(innerMessage);
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
                string innerMessage = GetInnerMessage(contactDependencyException);

                return Problem(innerMessage);
            }
            catch (ContactServiceException contactServiceException)
            {
                string innerMessage = GetInnerMessage(contactServiceException);

                return Problem(innerMessage);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
