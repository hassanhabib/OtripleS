// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Tests.Acceptance.Models.Attendances
{
    public class Attendance
    {
        public Guid Id { get; set; }
        public Guid StudentSemesterCourseId { get; set; }
        public AttendanceStatus Status { get; set; }
        public DateTimeOffset AttendanceDate { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}