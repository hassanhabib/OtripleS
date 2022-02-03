// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnCreateWhenSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom someClassroom = CreateRandomClassroom(dateTime);
            var sqlException = GetSqlException();
            var failedClassroomStorageException = new FailedClassroomStorageException(sqlException);

            var expectedClassroomDependencyException =
                new ClassroomDependencyException(failedClassroomStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(someClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomDependencyException>(() =>
                createClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedClassroomDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClassroomAsync(It.IsAny<Classroom>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Classroom someClassroom = CreateRandomClassroom();
            var databseUpdateException = new DbUpdateException();

            var failedClassroomStorageException =
                new FailedClassroomStorageException(databseUpdateException);

            var expectedClassroomDependencyException =
                new ClassroomDependencyException(failedClassroomStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databseUpdateException);

            // when
            ValueTask<Classroom> addClassroomTask =
                this.classroomService.CreateClassroomAsync(someClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomDependencyException>(() =>
                addClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClassroomDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClassroomAsync(It.IsAny<Classroom>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom inputClassroom = randomClassroom;
            inputClassroom.UpdatedBy = inputClassroom.CreatedBy;
            inputClassroom.UpdatedDate = inputClassroom.CreatedDate;
            var exception = new Exception();

            var expectedClassroomServiceException =
                new ClassroomServiceException(exception);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertClassroomAsync(inputClassroom))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Classroom> createClassroomTask =
                 this.classroomService.CreateClassroomAsync(inputClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomServiceException>(() =>
                createClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClassroomServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClassroomAsync(inputClassroom),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}