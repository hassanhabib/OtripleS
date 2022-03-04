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

        [Fact]
        public void ShouldRetrieveAllTeacherAttachments()
        {
            // given
            IQueryable<TeacherAttachment> randomTeacherAttachments = CreateRandomTeacherAttachments();
            IQueryable<TeacherAttachment> storageTeacherAttachments = randomTeacherAttachments;
            IQueryable<TeacherAttachment> expectedTeacherAttachments = storageTeacherAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeacherAttachments())
                    .Returns(storageTeacherAttachments);

            // when
            IQueryable<TeacherAttachment> actualTeacherAttachments =
                this.teacherAttachmentService.RetrieveAllTeacherAttachments();

            // then
            actualTeacherAttachments.Should().BeEquivalentTo(expectedTeacherAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeacherAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

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
