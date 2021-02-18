// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someExamId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedExamAttachmentDependencyException =
                new ExamAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectExamAttachmentByIdAsync(someExamId, someAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ExamAttachment> removeExamAttachmentTask =
                this.ExamAttachmentService.RemoveExamAttachmentByIdAsync(
                    someExamId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<ExamAttachmentDependencyException>(() =>
                removeExamAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(someExamId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedExamAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamAttachmentAsync(It.IsAny<ExamAttachment>()),
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
            Guid someExamId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedExamAttachmentDependencyException =
                new ExamAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamAttachmentByIdAsync(someExamId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ExamAttachment> removeExamAttachmentTask =
                this.ExamAttachmentService.RemoveExamAttachmentByIdAsync
                (someExamId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<ExamAttachmentDependencyException>(() =>
                removeExamAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(someExamId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamAttachmentAsync(It.IsAny<ExamAttachment>()),
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
            Guid someExamId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedExamAttachmentException(databaseUpdateConcurrencyException);

            var expectedExamAttachmentException =
                new ExamAttachmentDependencyException(lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamAttachmentByIdAsync(someExamId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ExamAttachment> removeExamAttachmentTask =
                this.ExamAttachmentService.RemoveExamAttachmentByIdAsync(someExamId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<ExamAttachmentDependencyException>(() =>
                removeExamAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(someExamId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamAttachmentException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamAttachmentAsync(It.IsAny<ExamAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
