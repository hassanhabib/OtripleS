// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentRegistrations;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<StudentRegistration> StudentRegistrations { get; set; }

        public IQueryable<StudentRegistration> SelectAllStudentRegistrations() =>
            this.StudentRegistrations.AsQueryable();

        public async ValueTask<StudentRegistration> SelectStudentRegistrationByIdAsync(
            Guid studentId,
            Guid registrationId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.StudentRegistrations.FindAsync(studentId, registrationId);
        }
    }
}
