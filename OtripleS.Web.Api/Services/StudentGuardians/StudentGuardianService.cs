// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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

		public ValueTask<StudentGuardian> ModifyStudentGuardianAsync(StudentGuardian studentGuardian) =>
		TryCatch(async () =>
		{
			ValidateStudentGuardianOnModify(studentGuardian);

			StudentGuardian maybeStudentGuardian =
				await storageBroker.SelectStudentGuardianByIdAsync(studentGuardian.StudentId, studentGuardian.GuardianId);

			ValidateStorageStudentGuardian(
				maybeStudentGuardian,
				studentGuardian.StudentId,
				studentGuardian.GuardianId);

			ValidateAgainstStorageStudentGuardianOnModify(
				inputStudentGuardian: studentGuardian,
				storageStudentGuardian: maybeStudentGuardian);

			return await storageBroker.UpdateStudentGuardianAsync(studentGuardian);
		});
	}
}
