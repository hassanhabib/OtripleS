// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentAttachments;
using OtripleS.Web.Api.Models.StudentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedStudentAttachmentDependencyException
                = new StudentAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentAttachmentByIdAsync(someStudentId, someAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentAttachment> retrieveStudentAttachmentTask =
                this.studentAttachmentService.RetrieveStudentAttachmentByIdAsync
                (someStudentId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentDependencyException>(() =>
                retrieveStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(someStudentId, someAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentAttachmentDependencyException =
                new StudentAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentAttachmentByIdAsync(someStudentId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentAttachment> retrieveStudentAttachmentTask =
                this.studentAttachmentService.RetrieveStudentAttachmentByIdAsync
                (someStudentId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentDependencyException>(
                () => retrieveStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(someStudentId, someAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomStudentId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputStudentId = randomStudentId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedStudentAttachmentException(databaseUpdateConcurrencyException);

            var expectedStudentAttachmentException =
                new StudentAttachmentDependencyException(lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentAttachment> retrieveStudentAttachmentTask =
                this.studentAttachmentService.RetrieveStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentDependencyException>(() =>
                retrieveStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomStudentId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputStudentId = randomStudentId;
            var serviceException = new Exception();

            var failedStudentAttachmentServiceException =
                new FailedStudentAttachmentServiceException(serviceException);

            var expectedStudentAttachmentServiceException =
                new StudentAttachmentServiceException(failedStudentAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<StudentAttachment> retrieveStudentAttachmentTask =
                this.studentAttachmentService.RetrieveStudentAttachmentByIdAsync
                (inputStudentId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentServiceException>(() =>
                retrieveStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
