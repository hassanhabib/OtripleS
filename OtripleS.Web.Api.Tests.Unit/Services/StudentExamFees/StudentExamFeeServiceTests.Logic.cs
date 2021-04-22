// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
<<<<<<< HEAD
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;
=======
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
>>>>>>> ef731125589f73b5a7c937a68dc7df752e17ae8c
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
<<<<<<< HEAD
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
=======
        public async Task ShouldAddStudentExamFeeAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee();
            randomStudentExamFee.UpdatedBy = randomStudentExamFee.CreatedBy;
            randomStudentExamFee.UpdatedDate = randomStudentExamFee.CreatedDate;
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            StudentExamFee storageStudentExamFee = randomStudentExamFee;
            StudentExamFee expectedStudentExamFee = randomStudentExamFee;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamFeeAsync(inputStudentExamFee))
                    .ReturnsAsync(storageStudentExamFee);

            // when
            StudentExamFee actualStudentExamFee =
                await this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);
>>>>>>> ef731125589f73b5a7c937a68dc7df752e17ae8c

            // then
            actualStudentExamFee.Should().BeEquivalentTo(expectedStudentExamFee);

<<<<<<< HEAD
            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(inputStudentExamFeeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(storageStudentExamFee),
=======
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamFeeAsync(inputStudentExamFee),
>>>>>>> ef731125589f73b5a7c937a68dc7df752e17ae8c
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
<<<<<<< HEAD
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

=======
        public void ShouldRetrieveAllStudentExamFees()
        {
            // given
            IQueryable<StudentExamFee> randomStudentExamFees =
                CreateRandomStudentExamFees();

            IQueryable<StudentExamFee> storageStudentExamFees =
                randomStudentExamFees;

            IQueryable<StudentExamFee> expectedStudentExamFees =
                storageStudentExamFees;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentExamFees())
                    .Returns(storageStudentExamFees);

            // when
            IQueryable<StudentExamFee> actualStudentExamFees =
                this.studentExamFeeService.RetrieveAllStudentExamFees();

            // then
            actualStudentExamFees.Should().BeEquivalentTo(expectedStudentExamFees);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentExamFees(),
                    Times.Once);

>>>>>>> ef731125589f73b5a7c937a68dc7df752e17ae8c
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

<<<<<<< HEAD

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

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentExamFeeId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedStudentExamFeeException(databaseUpdateConcurrencyException);

            var expectedStudentExamFeeException =
                new StudentExamFeeDependencyException(lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdAsync(someStudentExamFeeId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentExamFee> removeStudentExamFeeTask =
                this.StudentExamFeeService.RemoveStudentExamFeeByIdAsync(someStudentExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeDependencyException>(() =>
                removeStudentExamFeeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(someStudentExamFeeId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentExamFeeId = Guid.NewGuid();
            var exception = new Exception();
            var expectedStudentExamFeeException = new StudentExamFeeServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdAsync(someStudentExamFeeId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentExamFee> removeStudentExamFeeTask =
                this.StudentExamFeeService.RemoveStudentExamFeeByIdAsync(
                    someStudentExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeServiceException>(() =>
                removeStudentExamFeeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(someStudentExamFeeId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


=======
>>>>>>> ef731125589f73b5a7c937a68dc7df752e17ae8c
    }
}
