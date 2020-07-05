// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Models.Students.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentServiceTests
{
    public partial class StudentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Student randomStudent = CreateRandomStudent();
            Student storageStudent = randomStudent;
            var sqlException = CreateSqlException();

            var expectedStudentDependencyException =
                new StudentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentByIdAsync(inputStudentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Student> retrieveStudentByIdTask =
                this.studentService.RetrieveStudentByIdAsync(inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                retrieveStudentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(inputStudentId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentDependencyException =
                new StudentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentByIdAsync(inputStudentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Student> retrieveStudentByIdTask =
                this.studentService.RetrieveStudentByIdAsync(inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                retrieveStudentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(inputStudentId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            var exception = new Exception();

            var expectedStudentServiceException =
                new StudentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentByIdAsync(inputStudentId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Student> retrieveStudentByIdTask =
                this.studentService.RetrieveStudentByIdAsync(inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentServiceException>(() =>
                retrieveStudentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(inputStudentId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
