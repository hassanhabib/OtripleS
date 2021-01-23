// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.TeacherAttachments;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.TeacherAttachments
{
    public partial class TeacherAttachmentsApiTests
    {
        [Fact]
        public async Task ShouldPostTeacherAttachmentAsync()
        {
            // given
            TeacherAttachment randomTeacherAttachment = await CreateRandomTeacherAttachment();
            TeacherAttachment inputTeacherAttachment = randomTeacherAttachment;
            TeacherAttachment expectedTeacherAttachment = inputTeacherAttachment;

            // when             
            await this.otripleSApiBroker.PostTeacherAttachmentAsync(inputTeacherAttachment);

            TeacherAttachment actualTeacherAttachment =
                await this.otripleSApiBroker.GetTeacherAttachmentByIdsAsync(
                    inputTeacherAttachment.TeacherId,
                    inputTeacherAttachment.AttachmentId);

            // then
            actualTeacherAttachment.Should().BeEquivalentTo(expectedTeacherAttachment);
            await DeleteTeacherAttachmentAsync(actualTeacherAttachment);
        }
    }
}
