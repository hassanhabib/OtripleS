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
using OtripleS.Web.Api.Models.Exams;

namespace OtripleS.Web.Api.Services.Foundations.Exams
{
    public partial class ExamService : IExamService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ExamService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker
            )
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Exam> RetrieveExamByIdAsync(Guid examId) =>
        TryCatch(async () =>
        {
            ValidateExamId(examId);

            Exam maybeExam =
                await this.storageBroker.SelectExamByIdAsync(examId);
            ValidateStorageExam(maybeExam, examId);

            return maybeExam;
        });

        public IQueryable<Exam> RetrieveAllExams() =>
        TryCatch(() => this.storageBroker.SelectAllExams());

        public ValueTask<Exam> AddExamAsync(Exam exam) =>
        TryCatch(async () =>
        {
            ValidateExamOnAdd(exam);

            return await this.storageBroker.InsertExamAsync(exam);
        });

        public ValueTask<Exam> RemoveExamByIdAsync(Guid examId) =>
        TryCatch(async () =>
        {
            ValidateExamId(examId);
            Exam maybeExam = await storageBroker.SelectExamByIdAsync(examId);
            ValidateStorageExam(maybeExam, examId);

            return await storageBroker.DeleteExamAsync(maybeExam);
        });

        public ValueTask<Exam> ModifyExamAsync(Exam exam) =>
        TryCatch(async () =>
        {
            ValidateExamOnModify(exam);
            Exam maybeExam = await storageBroker.SelectExamByIdAsync(exam.Id);
            ValidateStorageExam(maybeExam, exam.Id);
            ValidateAgainstStorageExamOnModify(inputExam: exam, storageExam: maybeExam);

            return await storageBroker.UpdateExamAsync(exam);
        });
    }
}
