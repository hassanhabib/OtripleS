// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentRegistrations;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveStudentRegistrationByIdAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid randomRegistrationId = Guid.NewGuid();
            Guid inputRegistrationId = randomRegistrationId;
            StudentRegistration randomStudentRegistration = CreateRandomStudentRegistration();
            StudentRegistration storageStudentRegistration = randomStudentRegistration;
            StudentRegistration expectedStudentRegistration = storageStudentRegistration;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId))
                    .ReturnsAsync(storageStudentRegistration);

            // when
            StudentRegistration actualStudentExam =
                await this.studentRegistrationService.RetrieveStudentRegistrationByIdAsync(
                    inputStudentId,
                    inputRegistrationId);

            // then
            actualStudentExam.Should().BeEquivalentTo(expectedStudentRegistration);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
