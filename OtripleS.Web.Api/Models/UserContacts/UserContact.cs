using System;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.Users;

namespace OtripleS.Web.Api.Models.UserContacts
{
    public class UserContact
    {
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
