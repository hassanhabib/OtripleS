// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
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
        public void ShouldRetrieveAllStudentRegistrations()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<StudentRegistration> randomStudentRegistrations =
                CreateRandomStudentRegistrations(randomDateTime);

            IQueryable<StudentRegistration> storageStudentRegistrations =
                randomStudentRegistrations;

            IQueryable<StudentRegistration> expectedStudentRegistrations =
                storageStudentRegistrations;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentRegistrations())
                    .Returns(storageStudentRegistrations);

            // when
            IQueryable<StudentRegistration> actualStudentRegistrations =
                this.studentRegistrationService.RetrieveAllStudentRegistrations();

            // then
            actualStudentRegistrations.Should().BeEquivalentTo(expectedStudentRegistrations);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentRegistrations(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
        public async Task ShouldRetrieveStudentRegistrationByIdAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid randomRegistrationId = Guid.NewGuid();
            Guid inputRegistrationId = randomRegistrationId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentRegistration randomStudentRegistration = CreateRandomStudentRegistration(randomDateTime);
            StudentRegistration storageStudentRegistration = randomStudentRegistration;
            StudentRegistration expectedStudentRegistration = storageStudentRegistration;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentId,inputRegistrationId))
                    .ReturnsAsync(storageStudentRegistration);

            // when
            StudentRegistration actualStudentExam =
                await this.studentRegistrationService.RetrieveStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId);

            // then
            actualStudentExam.Should().BeEquivalentTo(expectedStudentRegistration);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
