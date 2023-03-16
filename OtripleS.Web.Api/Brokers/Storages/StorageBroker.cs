// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OtripleS.Web.Api.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        private async ValueTask<T> InsertAsync<T>(T @object)
        {
            this.Entry(@object).State = EntityState.Added;
            await this.SaveChangesAsync();

            return @object;
        }

        private IQueryable<T> SelectAll<T>() where T : class => this.Set<T>();

        private async ValueTask<T> SelectAsync<T>(params object[] @objectIds) where T : class =>
            await this.FindAsync<T>(objectIds);

        private async ValueTask<T> UpdateAsync<T>(T @object)
        {
            this.Entry(@object).State = EntityState.Modified;
            await this.SaveChangesAsync();

            return @object;
        }

        private async ValueTask<T> DeleteAsync<T>(T @object)
        {
            this.Entry(@object).State = EntityState.Deleted;
            await this.SaveChangesAsync();

            return @object;
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
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            string connectionString = this.configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
