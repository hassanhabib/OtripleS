// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentAttachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldAddStudentAttachmentAsync()
        {
            // given
            StudentAttachment randomStudentAttachment = CreateRandomStudentAttachment();
            StudentAttachment inputStudentAttachment = randomStudentAttachment;
            StudentAttachment storageStudentAttachment = randomStudentAttachment;
            StudentAttachment expectedStudentAttachment = storageStudentAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAttachmentAsync(inputStudentAttachment))
                    .ReturnsAsync(storageStudentAttachment);

            // when
            StudentAttachment actualStudentAttachment =
                await this.studentAttachmentService.AddStudentAttachmentAsync(inputStudentAttachment);

            // then
            actualStudentAttachment.Should().BeEquivalentTo(expectedStudentAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAttachmentAsync(inputStudentAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
