// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.StudentGuardians;

namespace OtripleS.Web.Api.Services.Foundations.StudentGuardians
{
    public partial class StudentGuardianService : IStudentGuardianService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public StudentGuardianService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<StudentGuardian> AddStudentGuardianAsync(StudentGuardian studentGuardian) =>
        TryCatch(async () =>
        {
            ValidateStudentGuardianOnCreate(studentGuardian);

            return await this.storageBroker.InsertStudentGuardianAsync(studentGuardian);
        });
        
        public IQueryable<StudentGuardian> RetrieveAllStudentGuardians() =>
        TryCatch(() => this.storageBroker.SelectAllStudentGuardians());

        public ValueTask<StudentGuardian> RetrieveStudentGuardianByIdAsync(Guid studentId, Guid guardianId) =>
        TryCatch(async () =>
        {
            ValidateStudentGuardianIdIsNull(studentId, guardianId);
            StudentGuardian storageStudentGuardian =
                await this.storageBroker.SelectStudentGuardianByIdAsync(studentId, guardianId);
            ValidateStorageStudentGuardian(storageStudentGuardian, studentId, guardianId);
            return storageStudentGuardian;
        });

        public ValueTask<StudentGuardian> ModifyStudentGuardianAsync(StudentGuardian studentGuardian) =>
        TryCatch(async () =>
        {
            ValidateStudentGuardianOnModify(studentGuardian);

            StudentGuardian maybeStudentGuardian =
                await storageBroker.SelectStudentGuardianByIdAsync(
                studentGuardian.StudentId,
                studentGuardian.GuardianId);

            ValidateStorageStudentGuardian(
               maybeStudentGuardian,
               studentGuardian.StudentId,
               studentGuardian.GuardianId);

            ValidateAgainstStorageStudentGuardianOnModify(
               inputStudentGuardian: studentGuardian,
               storageStudentGuardian: maybeStudentGuardian);

            return await storageBroker.UpdateStudentGuardianAsync(studentGuardian);
        });
        
        public ValueTask<StudentGuardian> RemoveStudentGuardianByIdsAsync(Guid guardianId, Guid studentId) => TryCatch(async () =>
        {
            ValidateStudentGuardianId(guardianId);
            ValidateStudentId(studentId);

            StudentGuardian studentGuardian =
                await this.storageBroker.SelectStudentGuardianByIdAsync(guardianId, studentId);

            ValidateStorageStudentGuardian(studentGuardian, guardianId, studentId);

            return await this.storageBroker.DeleteStudentGuardianAsync(studentGuardian);
        });
    }
}
