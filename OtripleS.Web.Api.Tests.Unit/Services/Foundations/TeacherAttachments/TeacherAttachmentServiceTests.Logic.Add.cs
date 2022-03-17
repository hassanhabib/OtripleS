// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
        public async Task ShouldAddTeacherAttachmentAsync()
        {
            // given
            TeacherAttachment randomTeacherAttachment = CreateRandomTeacherAttachment();
            TeacherAttachment inputTeacherAttachment = randomTeacherAttachment;
            TeacherAttachment storageTeacherAttachment = randomTeacherAttachment;
            TeacherAttachment expectedTeacherAttachment = storageTeacherAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherAttachmentAsync(inputTeacherAttachment))
                    .ReturnsAsync(storageTeacherAttachment);

            // when
            TeacherAttachment actualTeacherAttachment =
                await this.teacherAttachmentService.AddTeacherAttachmentAsync(inputTeacherAttachment);

            // then
            actualTeacherAttachment.Should().BeEquivalentTo(expectedTeacherAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAttachmentAsync(inputTeacherAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
