//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            StudentAttachment randomStudentAttachment = CreateRandomStudentAttachment();
            StudentAttachment inputStudentAttachment = randomStudentAttachment;
            var sqlException = GetSqlException();

            var expectedStudentAttachmentDependencyException =
                new StudentAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAttachmentAsync(inputStudentAttachment))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentAttachment> addStudentAttachmentTask =
                this.studentAttachmentService.AddStudentAttachmentAsync(inputStudentAttachment);

            // then
            await Assert.ThrowsAsync<StudentAttachmentDependencyException>(() =>
                addStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAttachmentAsync(inputStudentAttachment),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            StudentAttachment randomStudentAttachment = CreateRandomStudentAttachment();
            StudentAttachment inputStudentAttachment = randomStudentAttachment;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentAttachmentDependencyException =
                new StudentAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAttachmentAsync(inputStudentAttachment))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentAttachment> addStudentAttachmentTask =
                this.studentAttachmentService.AddStudentAttachmentAsync(inputStudentAttachment);

            // then
            await Assert.ThrowsAsync<StudentAttachmentDependencyException>(() =>
                addStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAttachmentAsync(inputStudentAttachment),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            StudentAttachment randomStudentAttachment = CreateRandomStudentAttachment();
            StudentAttachment inputStudentAttachment = randomStudentAttachment;
            var serviceException = new Exception();

            var expectedStudentAttachmentServiceException =
                new StudentAttachmentServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAttachmentAsync(inputStudentAttachment))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<StudentAttachment> addStudentAttachmentTask =
                 this.studentAttachmentService.AddStudentAttachmentAsync(inputStudentAttachment);

            // then
            await Assert.ThrowsAsync<StudentAttachmentServiceException>(() =>
                addStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAttachmentAsync(inputStudentAttachment),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
