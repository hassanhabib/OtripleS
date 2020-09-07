using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Guardian
{
    public class Guardian : IAuditable
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public GuardianRelationship Relationship { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
