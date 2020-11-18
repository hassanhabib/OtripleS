// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExams
{
	public partial class StudentExamServiceTests
	{
		[Fact]
		public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
		{
			// given
			Guid randomStudentExamId = Guid.NewGuid();
			Guid inputStudentExamId = randomStudentExamId;
			SqlException sqlException = GetSqlException();

			var expectedStudentExamDependencyException = new StudentExamDependencyException(sqlException);

			this.storageBrokerMock.Setup(broker =>
				 broker.SelectStudentExamByIdAsync(inputStudentExamId))
					.ThrowsAsync(sqlException);

			// when
			ValueTask<StudentExam> deleteStudentExamTask =
				this.studentExamService.DeleteStudentExamByIdAsync(inputStudentExamId);

			// then
			await Assert.ThrowsAsync<StudentExamDependencyException>(() =>
				deleteStudentExamTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(expectedStudentExamDependencyException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectStudentExamByIdAsync(inputStudentExamId),
					Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
