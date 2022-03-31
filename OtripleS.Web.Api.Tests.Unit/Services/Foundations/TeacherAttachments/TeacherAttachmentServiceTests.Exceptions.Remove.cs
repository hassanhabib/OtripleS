// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.TeacherAttachments;
using OtripleS.Web.Api.Models.TeacherAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.TeacherAttachments
{
    public partial class TeacherAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someTeacherId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedTeacherAttachmentDependencyException =
                new TeacherAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<TeacherAttachment> removeTeacherAttachmentTask =
                this.teacherAttachmentService.RemoveTeacherAttachmentByIdAsync(
                    someTeacherId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentDependencyException>(() =>
                removeTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTeacherAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeacherAttachmentAsync(It.IsAny<TeacherAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someTeacherId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedTeacherAttachmentDependencyException =
                new TeacherAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<TeacherAttachment> removeTeacherAttachmentTask =
                this.teacherAttachmentService.RemoveTeacherAttachmentByIdAsync
                (someTeacherId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentDependencyException>(() =>
                removeTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeacherAttachmentAsync(It.IsAny<TeacherAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someTeacherId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedTeacherAttachmentException(databaseUpdateConcurrencyException);

            var expectedTeacherAttachmentException =
                new TeacherAttachmentDependencyException(lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<TeacherAttachment> removeTeacherAttachmentTask =
                this.teacherAttachmentService.RemoveTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentDependencyException>(() =>
                removeTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeacherAttachmentAsync(It.IsAny<TeacherAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            var randomAttachmentId = Guid.NewGuid();
            var randomTeacherId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someTeacherId = randomTeacherId;
            var serviceException = new Exception();

            var failedTeacherAttachmentServiceException =
                new FailedTeacherAttachmentServiceException(serviceException);

            var expectedTeacherAttachmentException =
                new TeacherAttachmentServiceException(failedTeacherAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<TeacherAttachment> removeTeacherAttachmentTask =
                this.teacherAttachmentService.RemoveTeacherAttachmentByIdAsync(
                    someTeacherId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentServiceException>(() =>
                removeTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeacherAttachmentAsync(It.IsAny<TeacherAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}