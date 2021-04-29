//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.StudentExamFees;

namespace OtripleS.Web.Api.Services.StudentExamFees
{
    public partial class StudentExamFeeService : IStudentExamFeeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public StudentExamFeeService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<StudentExamFee> AddStudentExamFeeAsync(StudentExamFee studentExamFee) =>
        TryCatch(async () =>
        {
            ValidateStudentExamFeeOnCreate(studentExamFee);

            return await this.storageBroker.InsertStudentExamFeeAsync(studentExamFee);
        });

        public ValueTask<StudentExamFee> RetrieveStudentExamFeeByIdsAsync(
            Guid studentId,
            Guid examFeeId) =>
        TryCatch(async () =>
        {
            ValidateStudentExamFeeIdsAreNull(studentId, examFeeId);

            StudentExamFee storageStudentExamFee =
                await this.storageBroker.SelectStudentExamFeeByIdsAsync(
                    studentId,
                    examFeeId);

            ValidateStorageStudentExamFee(storageStudentExamFee, studentId, examFeeId);

            return storageStudentExamFee;
        });

        public IQueryable<StudentExamFee> RetrieveAllStudentExamFees() =>
        TryCatch(() =>
        {
            IQueryable<StudentExamFee> storageStudentExamFees =
                this.storageBroker.SelectAllStudentExamFees();

            ValidateStorageStudentExamFees(storageStudentExamFees);

            return storageStudentExamFees;
        });

        public ValueTask<StudentExamFee> RemoveStudentExamFeeByIdAsync(
            Guid studentId,
            Guid examFeeId) =>
        TryCatch(async () =>
        {
            ValidateStudentExamFeeIdsAreNull(studentId, examFeeId);

            StudentExamFee maybeStudentExamFee =
                await storageBroker.SelectStudentExamFeeByIdsAsync(studentId, examFeeId);

            ValidateStorageStudentExamFee(maybeStudentExamFee, studentId, examFeeId);

            return await this.storageBroker.DeleteStudentExamFeeAsync(maybeStudentExamFee);
        });

        public ValueTask<StudentExamFee> ModifyStudentExamFeeAsync(StudentExamFee studentExamFee) =>
        TryCatch(async () =>
        {
            ValidateStudentExamFeeOnModify(studentExamFee);

            StudentExamFee maybeStudentExamFee =
                await storageBroker.SelectStudentExamFeeByIdsAsync(
                    studentExamFee.StudentId,
                    studentExamFee.ExamFeeId);

            ValidateStorageStudentExamFee(
                maybeStudentExamFee,
                studentExamFee.StudentId,
                studentExamFee.ExamFeeId);

            ValidateAgainstStorageStudentExamFeeOnModify(
                inputStudentExamFee: studentExamFee, storageStudentExamFee: maybeStudentExamFee);

            return await storageBroker.UpdateStudentExamFeeAsync(studentExamFee);
        });
    }
}
