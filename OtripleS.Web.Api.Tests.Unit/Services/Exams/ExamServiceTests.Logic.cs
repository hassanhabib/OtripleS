// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Exams;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Exams
{
    public partial class ExamServiceTests
	{
		[Fact]
		public async Task ShouldRetrieveExamByIdAsync()
		{
			// given
			Guid randomExamId = Guid.NewGuid();
			Guid inputExamId = randomExamId;
			DateTimeOffset randomDateTime = GetRandomDateTime();
			Exam randomExam = CreateRandomExam(randomDateTime);
			Exam storageExam = randomExam;
			Exam expectedExam = storageExam;

			this.storageBrokerMock.Setup(broker =>
				broker.SelectExamByIdAsync(inputExamId))
					.ReturnsAsync(storageExam);

			// when
			Exam actualExam =
				await this.examService.RetrieveExamByIdAsync(inputExamId);

			// then
			actualExam.Should().BeEquivalentTo(expectedExam);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Never);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectExamByIdAsync(inputExamId),
					Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public void ShouldRetrieveAllExams()
		{
			// given
			DateTimeOffset randomDateTime = GetRandomDateTime();
			IQueryable<Exam> randomExams = CreateRandomExams(randomDateTime);
			IQueryable<Exam> storageExams = randomExams;
			IQueryable<Exam> expectedExams = storageExams;

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAllExams())
					.Returns(storageExams);

			// when
			IQueryable<Exam> actualExams =
				this.examService.RetrieveAllExams();

			// then
			actualExams.Should().BeEquivalentTo(expectedExams);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Never);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAllExams(),
					Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldAddExamAsync()
		{
			// given
			DateTimeOffset randomDateTime = GetRandomDateTime();
			DateTimeOffset dateTime = randomDateTime;
			Exam randomExam = CreateRandomExam(randomDateTime);
			randomExam.UpdatedBy = randomExam.CreatedBy;
			Exam inputExam = randomExam;
			Exam storageExam = randomExam;
			Exam expectedExam = storageExam;

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTime())
					.Returns(dateTime);

			this.storageBrokerMock.Setup(broker =>
				broker.InsertExamAsync(inputExam))
					.ReturnsAsync(storageExam);

			// when
			Exam actualExam =
				await this.examService.AddExamAsync(inputExam);

			// then
			actualExam.Should().BeEquivalentTo(expectedExam);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertExamAsync(inputExam),
					Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldModifyExamAsync()
		{
			// given
			int randomNumber = GetRandomNumber();
			int randomDays = randomNumber;
			DateTimeOffset randomDate = GetRandomDateTime();
			DateTimeOffset randomInputDate = GetRandomDateTime();
			Exam randomExam = CreateRandomExam(randomInputDate);
			Exam inputExam = randomExam;
			Exam afterUpdateStorageExam = inputExam;
			Exam expectedExam = afterUpdateStorageExam;
			Exam beforeUpdateStorageExam = randomExam.DeepClone();
			inputExam.UpdatedDate = randomDate;
			Guid examId = inputExam.Id;

			this.dateTimeBrokerMock.Setup(broker =>
			   broker.GetCurrentDateTime())
				   .Returns(randomDate);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectExamByIdAsync(examId))
					.ReturnsAsync(beforeUpdateStorageExam);

			this.storageBrokerMock.Setup(broker =>
				broker.UpdateExamAsync(inputExam))
					.ReturnsAsync(afterUpdateStorageExam);

			// when
			Exam actualExam =
				await this.examService.ModifyExamAsync(inputExam);

			// then
			actualExam.Should().BeEquivalentTo(expectedExam);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectExamByIdAsync(examId),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.UpdateExamAsync(inputExam),
					Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldDeleteExamByIdAsync()
		{
			// given
			DateTimeOffset dateTime = GetRandomDateTime();
			Exam randomExam = CreateRandomExam(dateTime);
			Guid inputExamId = randomExam.Id;
			Exam inputExam = randomExam;
			Exam storageExam = randomExam;
			Exam expectedExam = randomExam;

			this.storageBrokerMock.Setup(broker =>
				broker.SelectExamByIdAsync(inputExamId))
					.ReturnsAsync(inputExam);

			this.storageBrokerMock.Setup(broker =>
				broker.DeleteExamAsync(inputExam))
					.ReturnsAsync(storageExam);

			// when
			Exam actualExam =
				await this.examService.DeleteExamByIdAsync(inputExamId);

			// then
			actualExam.Should().BeEquivalentTo(expectedExam);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectExamByIdAsync(inputExamId),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteExamAsync(inputExam),
					Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
