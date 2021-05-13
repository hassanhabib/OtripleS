// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using EFxceptions.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OtripleS.Web.Api.Models.Users;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsIdentityContext<User, Role, Guid>, IStorageBroker
    {
        private readonly IConfiguration configuration;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AddSemesterCourseReferences(modelBuilder);
            AddStudentSemesterCourseReferences(modelBuilder);
            AddStudentGuardianReferences(modelBuilder);
            AddStudentContactReferences(modelBuilder);
            AddTeacherContactReferences(modelBuilder);
            AddGuardianContactReferences(modelBuilder);
            AddUserContactReferences(modelBuilder);
            AddExamReferences(modelBuilder);
            AddStudentExamReferences(modelBuilder);
            AddCalendarEntryReferences(modelBuilder);
            AddStudentAttachmentReferences(modelBuilder);
            AddGuardianAttachmentReferences(modelBuilder);
            AddTeacherAttachmentReferences(modelBuilder);
            AddCalendarEntryAttachmentReferences(modelBuilder);
            AddCourseAttachmentReferences(modelBuilder);
            AddExamAttachmentReferences(modelBuilder);
            AddAssignmentAttachmentReferences(modelBuilder);
            AddFeeReferences(modelBuilder);
            AddExamFeeReferences(modelBuilder);
            AddStudentExamFeeReferences(modelBuilder);
            AddRegistrationReferences(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = this.configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
