// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.TeacherAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.TeacherAttachments
{
    public partial class TeacherAttachmentServiceTests
    {                

        [Fact]
        public async Task ShouldRetrieveTeacherAttachmentByIdAsync()
        {
            // given
            TeacherAttachment randomTeacherAttachment = CreateRandomTeacherAttachment();
            TeacherAttachment storageTeacherAttachment = randomTeacherAttachment;
            TeacherAttachment expectedTeacherAttachment = storageTeacherAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherAttachmentByIdAsync
                (randomTeacherAttachment.TeacherId, randomTeacherAttachment.AttachmentId))
                    .ReturnsAsync(storageTeacherAttachment);

            // when
            TeacherAttachment actualTeacherAttachment = await
                this.teacherAttachmentService.RetrieveTeacherAttachmentByIdAsync(
                    randomTeacherAttachment.TeacherId, randomTeacherAttachment.AttachmentId);

            // then
            actualTeacherAttachment.Should().BeEquivalentTo(expectedTeacherAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync
                (randomTeacherAttachment.TeacherId, randomTeacherAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
        
    }
}
