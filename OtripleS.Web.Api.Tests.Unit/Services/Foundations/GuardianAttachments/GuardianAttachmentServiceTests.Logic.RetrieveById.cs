﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.GuardianAttachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianAttachments
{
    public partial class GuardianAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveGuardianAttachmentById()
        {
            // given
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment();
            GuardianAttachment storageGuardianAttachment = randomGuardianAttachment;
            GuardianAttachment expectedGuardianAttachment = storageGuardianAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianAttachmentByIdAsync
                (randomGuardianAttachment.GuardianId, randomGuardianAttachment.AttachmentId))
                    .ReturnsAsync(storageGuardianAttachment);

            // when
            GuardianAttachment actualGuardianAttachment = await
                this.guardianAttachmentService.RetrieveGuardianAttachmentByIdAsync(
                    randomGuardianAttachment.GuardianId, randomGuardianAttachment.AttachmentId);

            // then
            actualGuardianAttachment.Should().BeEquivalentTo(expectedGuardianAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync
                (randomGuardianAttachment.GuardianId, randomGuardianAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
