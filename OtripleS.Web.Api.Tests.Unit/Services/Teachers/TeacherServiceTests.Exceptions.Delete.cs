// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Teachers
{
    public partial class TeacherServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomTeacherId = Guid.NewGuid();
            Guid inputTeacherId = randomTeacherId;
            SqlException sqlException = GetSqlException();

            var expectedTeacherDependencyException =
                new TeacherDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Teacher> deleteTeacherTask =
                this.teacherService.DeleteTeacherByIdAsync(inputTeacherId);

            // then
            await Assert.ThrowsAsync<TeacherDependencyException>(() =>
                deleteTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedTeacherDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomTeacherId = Guid.NewGuid();
            Guid inputTeacherId = randomTeacherId;
            var databaseUpdateException = new DbUpdateException();

            var expectedTeacherDependencyException =
                new TeacherDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Teacher> deleteTeacherTask =
                this.teacherService.DeleteTeacherByIdAsync(inputTeacherId);

            // then
            await Assert.ThrowsAsync<TeacherDependencyException>(() =>
                deleteTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomTeacherId = Guid.NewGuid();
            Guid inputTeacherId = randomTeacherId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedTeacherException = new LockedTeacherException(databaseUpdateConcurrencyException);

            var expectedTeacherDependencyException =
                new TeacherDependencyException(lockedTeacherException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Teacher> deleteTeacherTask =
                this.teacherService.DeleteTeacherByIdAsync(inputTeacherId);

            // then
            await Assert.ThrowsAsync<TeacherDependencyException>(() =>
                deleteTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomTeacherId = Guid.NewGuid();
            Guid inputTeacherId = randomTeacherId;
            var exception = new Exception();

            var expectedTeacherServiceException =
                new TeacherServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Teacher> deleteTeacherTask =
                this.teacherService.DeleteTeacherByIdAsync(inputTeacherId);

            // then
            await Assert.ThrowsAsync<TeacherServiceException>(() =>
                deleteTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
