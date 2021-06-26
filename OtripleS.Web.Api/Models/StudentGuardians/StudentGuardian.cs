// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Guardians;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Models.StudentGuardians
{
    public class StudentGuardian : IAuditable
    {
        public Guid GuardianId { get; set; }
        public Guardian Guardian { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public GuardianStudentRelationship Relationship { get; set; }
        public bool IsPrimaryContact { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
