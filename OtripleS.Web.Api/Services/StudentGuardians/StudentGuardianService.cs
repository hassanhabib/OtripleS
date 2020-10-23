// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.StudentGuardians;

namespace OtripleS.Web.Api.Services.StudentGuardians
{
    public partial class StudentGuardianService : IStudentGuardianService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public StudentGuardianService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<StudentGuardian> AddStudentGuardianAsync(StudentGuardian studentGuardian) =>
        TryCatch(async () =>
        {
            ValidateStudentGuardianOnCreate(studentGuardian);

            return await this.storageBroker.InsertStudentGuardianAsync(studentGuardian);
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

        public IQueryable<StudentGuardian> RetrieveAllStudentGuardians() =>
        TryCatch(() =>
        {
            IQueryable<StudentGuardian> storageStudentGuardians =
                this.storageBroker.SelectAllStudentGuardians();

            ValidateStorageStudentGuardians(storageStudentGuardians);

            return storageStudentGuardians;
        });

        public ValueTask<StudentGuardian> RetrieveStudentGuardianByIdAsync(Guid studentId, Guid guardianId) =>
        TryCatch(async () =>
        {
            ValidateStudentGuardianIdIsNull(studentId, guardianId);
            StudentGuardian storageStudentGuardian =
                await this.storageBroker.SelectStudentGuardianByIdAsync(studentId, guardianId);
            ValidateStorageStudentGuardian(storageStudentGuardian, studentId, guardianId);
            return storageStudentGuardian;
        });

        public ValueTask<StudentGuardian>
        DeleteStudentGuardianAsync(Guid GuardianId, Guid studentId) =>
        TryCatch(async () =>
        {
            ValidateStudentGuardianId(GuardianId);
            ValidateStudentId(studentId);

            StudentGuardian studentGuardian =
                await this.storageBroker.SelectStudentGuardianByIdAsync(GuardianId, studentId);

            ValidateStorageStudentGuardian(studentGuardian, GuardianId, studentId);

            return await this.storageBroker.DeleteStudentGuardianAsync(studentGuardian);
        });
    }
}
