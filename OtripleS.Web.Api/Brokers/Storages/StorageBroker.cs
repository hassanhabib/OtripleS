﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
            SetSemesterCourseReferences(modelBuilder);
            SetStudentSemesterCourseReferences(modelBuilder);
            SetStudentGuardianReferences(modelBuilder);
            SetStudentContactReferences(modelBuilder);
            SetTeacherContactReferences(modelBuilder);
            SetGuardianContactReferences(modelBuilder);
            SetUserContactReferences(modelBuilder);
            SetExamReferences(modelBuilder);
            SetStudentExamReferences(modelBuilder);
            SetCalendarEntryReferences(modelBuilder);
            SetStudentAttachmentReferences(modelBuilder);
            SetGuardianAttachmentReferences(modelBuilder);
            SetTeacherAttachmentReferences(modelBuilder);
            SetCalendarEntryAttachmentReferences(modelBuilder);
            SetCourseAttachmentReferences(modelBuilder);
            SetExamAttachmentReferences(modelBuilder);
            SetAssignmentAttachmentReferences(modelBuilder);
            SetFeeReferences(modelBuilder);
            SetExamFeeReferences(modelBuilder);
            SetStudentExamFeeReferences(modelBuilder);
            SetRegistrationReferences(modelBuilder);
            SetStudentRegistrationReferences(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = this.configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
