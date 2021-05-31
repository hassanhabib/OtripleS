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
          
        [Fact]
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
                broker.SelectStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId))
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

        [Fact]
        public async Task ShouldRemoveStudentRegistrationByIdsAsync()
        {
            // given
            var randomStudentId = Guid.NewGuid();
            var randomRegistrationId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid inputRegistrationId = randomRegistrationId;
            DateTimeOffset inputDateTime = GetRandomDateTime();
            StudentRegistration randomStudentRegistration = CreateRandomStudentRegistration(inputDateTime);
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
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
