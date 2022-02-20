// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Teachers
{
    public partial class TeacherServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {

            Teacher someTeacher = CreateRandomTeacher();
            var sqlException = GetSqlException();

            var failedTeacherStorageException =
                new FailedTeacherStorageException(sqlException);

            var expectedTeacherDependencyException =
                new TeacherDependencyException(failedTeacherStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Teacher> addTeacherTask =
                this.teacherService.CreateTeacherAsync(someTeacher);

            // then
            await Assert.ThrowsAsync<TeacherDependencyException>(() =>
                addTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAsync(It.IsAny<Teacher>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTeacherDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfTeacherAlreadyExistsAndLogItAsync()
        {
            // given
            var randomTeacher = CreateRandomTeacher();
            var alreadyExistsTeacher = randomTeacher;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsTeacherException =
                new AlreadyExistsTeacherException(duplicateKeyException);

            var expectedTeacherDependencyException =
                new TeacherDependencyValidationException(alreadyExistsTeacherException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(duplicateKeyException);

            // when 
            ValueTask<Teacher> addTeacherTask =
                this.teacherService.CreateTeacherAsync(alreadyExistsTeacher);

            // then
            await Assert.ThrowsAsync<TeacherDependencyValidationException>( () =>
                addTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAsync(It.IsAny<Teacher>()),
                    Times.Never());

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherDependencyException))), 
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorOccursAndLogItAsync()
        {
            // given
            Teacher someTeacher = CreateRandomTeacher();
            var databaseUpdateException = new DbUpdateException();

            var failedTeacherStorageException =
                new FailedTeacherStorageException(databaseUpdateException);

            var expectedTeacherDependencyException =
                new TeacherDependencyException(failedTeacherStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Teacher> AddTeacherTask =
                this.teacherService.CreateTeacherAsync(someTeacher);

            // then
            await Assert.ThrowsAsync<TeacherDependencyException>(() =>
                AddTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAsync(It.IsAny<Teacher>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Teacher someTeacher = CreateRandomTeacher();
            var serviceException = new Exception();

            var failedTeacherServiceException =
                new FailedTeacherServiceException(serviceException);

            var expectedTeacherServiceException =
                new TeacherServiceException(failedTeacherServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Teacher> createTeacherTask =
                 this.teacherService.CreateTeacherAsync(someTeacher);

            // then
            await Assert.ThrowsAsync<TeacherServiceException>(() =>
                createTeacherTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAsync(It.IsAny<Teacher>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
