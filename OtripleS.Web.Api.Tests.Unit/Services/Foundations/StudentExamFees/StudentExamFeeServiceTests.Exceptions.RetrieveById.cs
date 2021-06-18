// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Foundations.StudentExamFees;
using OtripleS.Web.Api.Models.Foundations.StudentExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentId = Guid.NewGuid();
            Guid someExamFeeId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var expectedStudentExamFeeDependencyException =
                new StudentExamFeeDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()))
                        .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentExamFee> retrieveStudentExamFeeByIdTask =
                this.studentExamFeeService.RetrieveStudentExamFeeByIdsAsync(
                    someStudentId, someExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeDependencyException>(() =>
                retrieveStudentExamFeeByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentExamFeeDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentId = Guid.NewGuid();
            Guid someExamFeeId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentExamFeeDependencyException =
                new StudentExamFeeDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()))
                        .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentExamFee> retrieveStudentExamFeeByIdTask =
                this.studentExamFeeService.RetrieveStudentExamFeeByIdsAsync(
                    someStudentId, someExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeDependencyException>(() =>
                retrieveStudentExamFeeByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentId = Guid.NewGuid();
            Guid someExamFeeId = Guid.NewGuid();
            var exception = new Exception();

            var expectedStudentExamFeeServiceException =
                new StudentExamFeeServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()))
                        .ThrowsAsync(exception);

            // when
            ValueTask<StudentExamFee> retrieveStudentExamFeeByIdTask =
                this.studentExamFeeService.RetrieveStudentExamFeeByIdsAsync(
                    someStudentId, someExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeServiceException>(() =>
                retrieveStudentExamFeeByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    It.IsAny<Guid>(), It.IsAny<Guid>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeServiceException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
