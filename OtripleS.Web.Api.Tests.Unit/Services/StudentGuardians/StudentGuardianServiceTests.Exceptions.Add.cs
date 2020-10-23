// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentGuardians
{
    public partial class StudentGuardianServiceTests
    {

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            inputStudentGuardian.UpdatedBy = inputStudentGuardian.CreatedBy;
            var sqlException = GetSqlException();

            var expectedStudentGuardianDependencyException =
                new StudentGuardianDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentGuardianAsync(inputStudentGuardian))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentGuardian> addStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianDependencyException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentGuardianDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(inputStudentGuardian),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            inputStudentGuardian.UpdatedBy = inputStudentGuardian.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentGuardianDependencyException =
                new StudentGuardianDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentGuardianAsync(inputStudentGuardian))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentGuardian> addStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianDependencyException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(inputStudentGuardian),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(dateTime);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            inputStudentGuardian.UpdatedBy = inputStudentGuardian.CreatedBy;
            var exception = new Exception();

            var expectedStudentGuardianServiceException =
                new StudentGuardianServiceException(exception);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentGuardianAsync(inputStudentGuardian))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentGuardian> addStudentGuardianTask =
                 this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianServiceException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(inputStudentGuardian),
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