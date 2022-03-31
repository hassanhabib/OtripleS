// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            TeacherAttachment randomTeacherAttachment = CreateRandomTeacherAttachment();
            TeacherAttachment inputTeacherAttachment = randomTeacherAttachment;
            var sqlException = GetSqlException();
            var expectedTeacherAttachmentDependencyException = new TeacherAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherAttachmentAsync(inputTeacherAttachment))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<TeacherAttachment> addTeacherAttachmentTask =
                this.teacherAttachmentService.AddTeacherAttachmentAsync(inputTeacherAttachment);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentDependencyException>(() =>
                addTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTeacherAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAttachmentAsync(inputTeacherAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            TeacherAttachment randomTeacherAttachment = CreateRandomTeacherAttachment();
            TeacherAttachment inputTeacherAttachment = randomTeacherAttachment;
            var databaseUpdateException = new DbUpdateException();

            var expectedTeacherAttachmentDependencyException =
                new TeacherAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherAttachmentAsync(inputTeacherAttachment))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<TeacherAttachment> addTeacherAttachmentTask =
                this.teacherAttachmentService.AddTeacherAttachmentAsync(inputTeacherAttachment);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentDependencyException>(() =>
                addTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAttachmentAsync(inputTeacherAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            TeacherAttachment randomTeacherAttachment = CreateRandomTeacherAttachment();
            TeacherAttachment inputTeacherAttachment = randomTeacherAttachment;
            var serviceException = new Exception();

            var failedTeacherAttachmentServiceException =
                new FailedTeacherAttachmentServiceException(serviceException);

            var expectedTeacherAttachmentServiceException =
                new TeacherAttachmentServiceException(failedTeacherAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherAttachmentAsync(It.IsAny<TeacherAttachment>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<TeacherAttachment> addTeacherAttachmentTask =
                 this.teacherAttachmentService.AddTeacherAttachmentAsync(inputTeacherAttachment);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentServiceException>(() =>
                addTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAttachmentAsync(It.IsAny<TeacherAttachment>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
