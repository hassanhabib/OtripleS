// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentRegistrations;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        [Fact]
        public async Task ShouldAddStudentRegistrationAsync()
        {
            // given
            StudentRegistration randomStudentRegistration = CreateRandomStudentRegistration();
            StudentRegistration inputStudentRegistration = randomStudentRegistration;
            StudentRegistration storageStudentRegistration = randomStudentRegistration;
            StudentRegistration expectedStudentRegistration = storageStudentRegistration;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentRegistrationAsync(inputStudentRegistration))
                    .ReturnsAsync(storageStudentRegistration);

            // when
            StudentRegistration actualStudentRegistration =
                await this.studentRegistrationService.AddStudentRegistrationAsync(inputStudentRegistration);

            // then
            actualStudentRegistration.Should().BeEquivalentTo(expectedStudentRegistration);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentRegistrationAsync(inputStudentRegistration),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
