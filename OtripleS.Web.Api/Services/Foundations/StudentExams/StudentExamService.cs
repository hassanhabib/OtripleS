// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.StudentExams;

namespace OtripleS.Web.Api.Services.Foundations.StudentExams
{
    public partial class StudentExamService : IStudentExamService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public StudentExamService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<StudentExam> AddStudentExamAsync(StudentExam studentExam) =>
        TryCatch(async () =>
        {
            ValidateStudentExamOnCreate(studentExam);

            return await this.storageBroker.InsertStudentExamAsync(studentExam);
        });

        public ValueTask<StudentExam> RetrieveStudentExamByIdAsync(Guid studentExamId) =>
        TryCatch(async () =>
        {
            ValidateStudentExamId(studentExamId);

            StudentExam maybeStudentExam =
                await this.storageBroker.SelectStudentExamByIdAsync(studentExamId);

            ValidateStorageStudentExam(maybeStudentExam, studentExamId);

            return maybeStudentExam;
        });

        public IQueryable<StudentExam> RetrieveAllStudentExams() =>
        TryCatch(() => this.storageBroker.SelectAllStudentExams());

        public ValueTask<StudentExam> ModifyStudentExamAsync(StudentExam studentExam) =>
        TryCatch(async () =>
        {
            ValidateStudentExamOnModify(studentExam);

            StudentExam maybeStudentExam =
               await storageBroker.SelectStudentExamByIdAsync(studentExam.Id);

            ValidateStorageStudentExam(maybeStudentExam, studentExam.Id);

            ValidateAgainstStorageStudentExamOnModify(
                inputStudentExam: studentExam, storageStudentExam: maybeStudentExam);

            return await storageBroker.UpdateStudentExamAsync(studentExam);
        });

        public ValueTask<StudentExam> RemoveStudentExamByIdAsync(Guid studentExamId) =>
        TryCatch(async () =>
        {
            ValidateStudentExamId(studentExamId);

            StudentExam maybeStudentExam =
               await this.storageBroker.SelectStudentExamByIdAsync(studentExamId);

            ValidateStorageStudentExam(maybeStudentExam, studentExamId);

            return await this.storageBroker.DeleteStudentExamAsync(maybeStudentExam);
        });
    }
}
