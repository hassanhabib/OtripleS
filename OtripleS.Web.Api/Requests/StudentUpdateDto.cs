using System;
using OtripleS.Web.Api.Models;

namespace OtripleS.Web.Api.Requests
{
    public class StudentUpdateDto
    {
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}