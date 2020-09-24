using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Contacts.Exceptions
{
    public class NotFoundContactException: Exception
    {
        public NotFoundContactException(Guid contactId)
            : base($"Couldn't find contact with Id: {contactId}.") { }
    }
}
