// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someExamId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedExamAttachmentDependencyException = 
                new ExamAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectExamAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ExamAttachment> retrieveExamAttachmentTask =
                this.examAttachmentService.RetrieveExamAttachmentByIdAsync(
                    someExamId, 
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<ExamAttachmentDependencyException>(() =>
                retrieveExamAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedExamAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        
    }
}