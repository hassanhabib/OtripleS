// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Tests.Acceptance.Models.Courses
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CourseStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
