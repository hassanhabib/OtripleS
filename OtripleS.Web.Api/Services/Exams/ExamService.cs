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
using OtripleS.Web.Api.Models.Exams;

namespace OtripleS.Web.Api.Services.Exams
{
    public partial class ExamService : IExamService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public ExamService(IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }


        public ValueTask<Exam> RetrieveExamByIdAsync(Guid examId) =>
        TryCatch(async () =>
        {
            ValidateExamId(examId);
            Exam storageExam = await this.storageBroker.SelectExamByIdAsync(examId);
            ValidateStorageExam(storageExam, examId);

            return storageExam;
        });

        public IQueryable<Exam> RetrieveAllExams() =>
        TryCatch(() =>
        {
            IQueryable<Exam> storageExams = this.storageBroker.SelectAllExams();
            ValidateStorageExams(storageExams);

            return storageExams;
        });

        public ValueTask<Exam> AddExamAsync(Exam exam) =>
        TryCatch(async () =>
        {
            ValidateExamOnAdd(exam);

            return await this.storageBroker.InsertExamAsync(exam);
        });

        public ValueTask<Exam> DeleteExamByIdAsync(Guid examId) =>
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
