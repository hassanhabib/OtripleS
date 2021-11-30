// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Classrooms
{
    public partial class ClassroomServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnDeleteWhenSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid randomClassroomId = Guid.NewGuid();
            Guid inputClassroomId = randomClassroomId;
            SqlException sqlException = GetSqlException();
            var failedClassroomStorageException = new FailedClassroomStorageException(sqlException);

            var expectedClassroomDependencyException =
                new ClassroomDependencyException(failedClassroomStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Classroom> deleteClassroomTask =
                this.classroomService.RemoveClassroomAsync(inputClassroomId);

            // then
            await Assert.ThrowsAsync<ClassroomDependencyException>(() =>
                deleteClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedClassroomDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someClassroomId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedClassroomException =
                new FailedClassroomStorageException(databaseUpdateException);

            var expectedClassroomDependencyException =
                new ClassroomDependencyException(failedClassroomException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClassroomByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Classroom> deleteClassroomTask =
                this.classroomService.RemoveClassroomAsync(someClassroomId);

            // then
            await Assert.ThrowsAsync<ClassroomDependencyException>(() =>
                deleteClassroomTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClassroomDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomClassroomId = Guid.NewGuid();
            Guid inputClassroomId = randomClassroomId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedClassroomException = new LockedClassroomException(databaseUpdateConcurrencyException);

            var expectedClassroomDependencyException =
                new ClassroomDependencyException(lockedClassroomException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Classroom> deleteClassroomTask =
                this.classroomService.RemoveClassroomAsync(inputClassroomId);

            // then
            await Assert.ThrowsAsync<ClassroomDependencyException>(() =>
                deleteClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomClassroomId = Guid.NewGuid();
            Guid inputClassroomId = randomClassroomId;
            var exception = new Exception();

            var expectedClassroomServiceException =
                new ClassroomServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Classroom> deleteClassroomTask =
                this.classroomService.RemoveClassroomAsync(inputClassroomId);

            // then
            await Assert.ThrowsAsync<ClassroomServiceException>(() =>
                deleteClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}