//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.TeacherAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherAttachments
{
    public partial class TeacherAttachmentServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenTeacherAttachmentsWereEmptyAndLogIt()
        {
            // given
            IQueryable<TeacherAttachment> emptyStorageTeacherAttachments = new List<TeacherAttachment>().AsQueryable();
            IQueryable<TeacherAttachment> expectedTeacherAttachments = emptyStorageTeacherAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeacherAttachments())
                    .Returns(expectedTeacherAttachments);

            // when
            IQueryable<TeacherAttachment> actualTeacherAttachments =
                this.teacherAttachmentService.RetrieveAllTeacherAttachments();

            // then
            actualTeacherAttachments.Should().BeEquivalentTo(emptyStorageTeacherAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeacherAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No teacher attachments found in storage."),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
