// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldRemoveStudentExamFeeAsync()
        {
            // given
            var randomStudentExamFeeId = Guid.NewGuid();
            Guid inputStudentExamFeeId = randomStudentExamFeeId;
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee();
            randomStudentExamFee.Id = inputStudentExamFeeId;
            StudentExamFee storageStudentExamFee = randomStudentExamFee;
            StudentExamFee expectedStudentExamFee = storageStudentExamFee;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdAsync(inputStudentExamFeeId))
                    .ReturnsAsync(storageStudentExamFee);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentExamFeeAsync(storageStudentExamFee))
                    .ReturnsAsync(expectedStudentExamFee);

            // when
            StudentExamFee actualStudentExamFee =
                await this.StudentExamFeeService.RemoveStudentExamFeeByIdAsync(inputStudentExamFeeId);

            // then
            actualStudentExamFee.Should().BeEquivalentTo(expectedStudentExamFee);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(inputStudentExamFeeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(storageStudentExamFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenStudentExamFeeIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentExamFeeId = default;
            Guid inputStudentExamFeeId = randomStudentExamFeeId;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.Id),
                parameterValue: inputStudentExamFeeId);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> removeStudentExamFeeTask =
                this.StudentExamFeeService.RemoveStudentExamFeeByIdAsync(inputStudentExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                removeStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageStudentExamFeeIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(randomDateTime);
            Guid inputStudentExamFeeId = randomStudentExamFee.Id;
            StudentExamFee nullStorageStudentExamFee = null;

            var notFoundStudentExamFeeException =
                new NotFoundStudentExamFeeException(inputStudentExamFeeId);

            var expectedAssignmentValidationException =
                new StudentExamFeeValidationException(notFoundStudentExamFeeException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentExamFeeByIdAsync(inputStudentExamFeeId))
                    .ReturnsAsync(nullStorageStudentExamFee);

            // when
            ValueTask<StudentExamFee> removeStudentExamFeeTask =
                this.StudentExamFeeService.RemoveStudentExamFeeByIdAsync(inputStudentExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                removeStudentExamFeeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid studentExamFeeId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedStudentExamFeeDependencyException =
                new StudentExamFeeDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentExamFeeByIdAsync(studentExamFeeId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentExamFee> removeStudentExamFeeTask =
                this.StudentExamFeeService.RemoveStudentExamFeeByIdAsync(
                    studentExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeDependencyException>(() =>
                removeStudentExamFeeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(studentExamFeeId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentExamFeeDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentExamFeeId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedstudentExamFeeIdDependencyException =
                new StudentExamFeeDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdAsync(someStudentExamFeeId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentExamFee> removestudentExamFeeIdTask =
                this.StudentExamFeeService.RemoveStudentExamFeeByIdAsync
                (someStudentExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeDependencyException>(() =>
                removestudentExamFeeIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(someStudentExamFeeId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedstudentExamFeeIdDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
