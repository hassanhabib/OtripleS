// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExams
{
	public partial class StudentExamServiceTests
	{
		[Fact]
		public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
		{
			// given
			Guid randomStudentExamId = default;
			Guid inputStudentExamId = randomStudentExamId;

			var invalidStudentExamInputException = new InvalidStudentExamInputException(
				parameterName: nameof(StudentExam.Id),
				parameterValue: inputStudentExamId);
			var expectedStudentExamValidationException =
				new StudentExamValidationException(invalidStudentExamInputException);

			//when
			ValueTask<StudentExam> actualStudentExamDeleteTask =
				this.studentExamService.DeleteStudentExamByIdAsync(inputStudentExamId);

			//then
			await Assert.ThrowsAsync<StudentExamValidationException>(
				() => actualStudentExamDeleteTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectStudentExamByIdAsync(It.IsAny<Guid>()),
					Times.Never);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteStudentExamAsync(It.IsAny<StudentExam>()),
					Times.Never);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageStudentExamIsInvalidAndLogItAsync()
		{
			// given
			DateTimeOffset randomDateTime = GetRandomDateTime();
			StudentExam randomStudentExam = CreateRandomStudentExam(randomDateTime);
			Guid inputStudentExamId = randomStudentExam.Id;
			StudentExam nullStorageStudentExam = null;

			var notFoundStudentExamException = new NotFoundStudentExamException(inputStudentExamId);

			var expectedStudentExamValidationException =
				new StudentExamValidationException(notFoundStudentExamException);

			this.storageBrokerMock.Setup(broker =>
				 broker.SelectStudentExamByIdAsync(inputStudentExamId))
					.ReturnsAsync(nullStorageStudentExam);

			// when
			ValueTask<StudentExam> actualStudentExamDeleteTask =
				this.studentExamService.DeleteStudentExamByIdAsync(inputStudentExamId);

			// then
			await Assert.ThrowsAsync<StudentExamValidationException>(() => actualStudentExamDeleteTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectStudentExamByIdAsync(inputStudentExamId),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteStudentExamAsync(It.IsAny<StudentExam>()),
					Times.Never);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
		{
			// given
			Guid randomStudentExamId = Guid.NewGuid();
			Guid inputStudentExamId = randomStudentExamId;
			var exception = new Exception();

			var expectedStudentExamServiceException = new StudentExamServiceException(exception);

			this.storageBrokerMock.Setup(broker =>
				 broker.SelectStudentExamByIdAsync(inputStudentExamId))
					.ThrowsAsync(exception);

			// when
			ValueTask<StudentExam> deleteStudentExamTask =
				this.studentExamService.DeleteStudentExamByIdAsync(inputStudentExamId);

			// then
			await Assert.ThrowsAsync<StudentExamServiceException>(() =>
				deleteStudentExamTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedStudentExamServiceException))),
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
