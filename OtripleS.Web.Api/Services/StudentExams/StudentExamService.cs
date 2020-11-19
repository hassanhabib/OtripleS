//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.StudentExams;
using System;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.StudentExams
{
    public partial class StudentExamService : IStudentExamService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public StudentExamService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
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

            StudentExam storageStudentExam =
                await this.storageBroker.SelectStudentExamByIdAsync(studentExamId);

            ValidateStorageStudentExam(storageStudentExam, studentExamId);

            return storageStudentExam;
        });
    }
}
