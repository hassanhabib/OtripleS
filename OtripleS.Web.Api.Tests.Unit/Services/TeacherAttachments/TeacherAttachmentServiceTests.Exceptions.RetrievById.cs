// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.TeacherAttachments;
using OtripleS.Web.Api.Models.TeacherAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherAttachments
{
    public partial class TeacherAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid randomTeacherId = Guid.NewGuid();
            Guid inputTeacherId = randomTeacherId;
            SqlException sqlException = GetSqlException();

            var expectedTeacherAttachmentDependencyException
                = new TeacherAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectTeacherAttachmentByIdAsync(inputTeacherId, inputAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<TeacherAttachment> deleteTeacherAttachmentTask =
                this.teacherAttachmentService.RetrieveTeacherAttachmentByIdAsync
                (inputTeacherId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentDependencyException>(() =>
                deleteTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedTeacherAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync(inputTeacherId, inputAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
