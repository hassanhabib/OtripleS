//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.StudentExams;

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

		public ValueTask<StudentExam> RetrieveStudentExamByIdAsync(Guid studentExamId) =>
		TryCatch(async () =>
		{
			ValidateStudentExamId(studentExamId);

			StudentExam storageStudentExam =
				await this.storageBroker.SelectStudentExamByIdAsync(studentExamId);

			ValidateStorageStudentExam(storageStudentExam, studentExamId);

			return storageStudentExam;
		});

		public ValueTask<StudentExam> DeleteStudentExamByIdAsync(Guid studentExamId) =>
		TryCatch(async () =>
		{
			ValidateStudentExamId(studentExamId);

			StudentExam maybeStudentExam =
			   await this.storageBroker.SelectStudentExamByIdAsync(studentExamId);

			ValidateStorageStudentExam(maybeStudentExam, studentExamId);

			return await this.storageBroker.DeleteStudentExamAsync(maybeStudentExam);
		});

        public ValueTask<StudentExam> CreateStudentExamAsync(StudentExam studentExam)
        {
            throw new NotImplementedException();
        }

		public IQueryable<StudentExam> RetrieveAllStudentExams() =>
		TryCatch(() =>
		{
			IQueryable<StudentExam> storageStudentExams =
				this.storageBroker.SelectAllStudentExams();

			ValidateStorageStudentExams(storageStudentExams);

			return storageStudentExams;
		});

        public async ValueTask<StudentExam> ModifyStudentExamAsync(StudentExam studentExam)
        {
			StudentExam maybeStudentExam =
			   await storageBroker.SelectStudentExamByIdAsync(studentExam.Id);

			return await storageBroker.UpdateStudentExamAsync(studentExam);
		}
    }
}
