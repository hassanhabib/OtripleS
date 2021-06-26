//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenStudentAttachmentsWereEmptyAndLogIt()
        {
            // given
            IQueryable<StudentAttachment> emptyStorageStudentAttachments = new List<StudentAttachment>().AsQueryable();
            IQueryable<StudentAttachment> expectedStudentAttachments = emptyStorageStudentAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentAttachments())
                    .Returns(expectedStudentAttachments);

            // when
            IQueryable<StudentAttachment> actualStudentAttachments =
                this.studentAttachmentService.RetrieveAllStudentAttachments();

            // then
            actualStudentAttachments.Should().BeEquivalentTo(emptyStorageStudentAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No student attachments found in storage."),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
