// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Exams
{
    public partial class ExamServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(randomDateTime);
            Exam someExam = randomExam;
            someExam.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedExamDependencyException =
                new ExamDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(someExam.Id))
                    .ThrowsAsync(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(someExam);

            // then
            await Assert.ThrowsAsync<ExamDependencyException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(someExam.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedExamDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(randomDateTime);
            Exam someExam = randomExam;
            someExam.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedExamDependencyException =
                new ExamDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(someExam.Id))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(someExam);

            // then
            await Assert.ThrowsAsync<ExamDependencyException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(someExam.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(randomDateTime);
            Exam someExam = randomExam;
            someExam.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedExamException = new LockedExamException(databaseUpdateConcurrencyException);

            var expectedExamDependencyException =
                new ExamDependencyException(lockedExamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(someExam.Id))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(someExam);

            // then
            await Assert.ThrowsAsync<ExamDependencyException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(someExam.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(randomDateTime);
            Exam someExam = randomExam;
            someExam.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var expectedExamServiceException =
                new ExamServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(someExam.Id))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(someExam);

            // then
            await Assert.ThrowsAsync<ExamServiceException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(someExam.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamServiceException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
