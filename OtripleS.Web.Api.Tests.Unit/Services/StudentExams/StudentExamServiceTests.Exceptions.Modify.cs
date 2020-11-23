// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExams
{
    public partial class StudentExamServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentExam randomStudentExam = CreateRandomStudentExam(randomDateTime);
            StudentExam someStudentExam = randomStudentExam;
            someStudentExam.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedStudentExamDependencyException =
                new StudentExamDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync(someStudentExam.Id))
                    .ThrowsAsync(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(someStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamDependencyException>(() =>
                modifyStudentExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentExamDependencyException))),
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
            StudentExam randomStudentExam = CreateRandomStudentExam(randomDateTime);
            StudentExam someStudentExam = randomStudentExam;
            someStudentExam.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentExamDependencyException =
                new StudentExamDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync(someStudentExam.Id))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(someStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamDependencyException>(() =>
                modifyStudentExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamDependencyException))),
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
            StudentExam randomStudentExam = CreateRandomStudentExam(randomDateTime);
            StudentExam someStudentExam = randomStudentExam;
            someStudentExam.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var expectedStudentExamServiceException =
                new StudentExamServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync(someStudentExam.Id))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(someStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamServiceException>(() =>
                modifyStudentExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamServiceException))),
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
            StudentExam randomStudentExam = CreateRandomStudentExam(randomDateTime);
            StudentExam someStudentExam = randomStudentExam;
            someStudentExam.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedStudentExamException = new LockedStudentExamException(databaseUpdateConcurrencyException);

            var expectedStudentExamDependencyException =
                new StudentExamDependencyException(lockedStudentExamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync
                (someStudentExam.Id))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(someStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamDependencyException>(() =>
                modifyStudentExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
