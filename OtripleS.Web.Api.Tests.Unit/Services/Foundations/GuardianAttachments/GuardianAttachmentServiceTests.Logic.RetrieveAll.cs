// ---------------------------------------------------------------
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
        public void ShouldRetrieveAllGuardianAttachments()
        {
            // given
            IQueryable<GuardianAttachment> randomGuardianAttachments = CreateRandomGuardianAttachments();
            IQueryable<GuardianAttachment> storageGuardianAttachments = randomGuardianAttachments;
            IQueryable<GuardianAttachment> expectedGuardianAttachments = storageGuardianAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuardianAttachments())
                    .Returns(storageGuardianAttachments);

            // when
            IQueryable<GuardianAttachment> actualGuardianAttachments =
                this.guardianAttachmentService.RetrieveAllGuardianAttachments();

            // then
            actualGuardianAttachments.Should().BeEquivalentTo(expectedGuardianAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuardianAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
