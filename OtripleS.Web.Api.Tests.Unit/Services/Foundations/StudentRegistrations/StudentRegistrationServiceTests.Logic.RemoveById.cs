// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
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
        public async Task ShouldRemoveStudentRegistrationByIdsAsync()
        {
            // given
            var randomStudentId = Guid.NewGuid();
            var randomRegistrationId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid inputRegistrationId = randomRegistrationId;
            StudentRegistration randomStudentRegistration = CreateRandomStudentRegistration();
            randomStudentRegistration.StudentId = inputStudentId;
            randomStudentRegistration.RegistrationId = inputRegistrationId;
            StudentRegistration storageStudentRegistration = randomStudentRegistration;
            StudentRegistration expectedStudentRegistration = storageStudentRegistration;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId))
                    .ReturnsAsync(storageStudentRegistration);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentRegistrationAsync(storageStudentRegistration))
                    .ReturnsAsync(expectedStudentRegistration);

            // when
            StudentRegistration actualStudentRegistration =
                await this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(inputStudentId, inputRegistrationId);

            // then
            actualStudentRegistration.Should().BeEquivalentTo(expectedStudentRegistration);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentRegistrationAsync(storageStudentRegistration),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
