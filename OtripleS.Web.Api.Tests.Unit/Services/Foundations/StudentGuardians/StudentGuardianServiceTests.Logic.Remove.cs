// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentGuardians
{
    public partial class StudentGuardianServiceTests
    { 
        [Fact]
        public async Task ShouldRemoveStudentGuardianAsync()
        {
            // given
            var randomStudentId = Guid.NewGuid();
            var randomGuardianId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid inputGuardianId = randomGuardianId;
            DateTimeOffset inputDateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(inputDateTime);
            randomStudentGuardian.StudentId = inputStudentId;
            randomStudentGuardian.GuardianId = inputGuardianId;
            StudentGuardian storageStudentGuardian = randomStudentGuardian;
            StudentGuardian expectedStudentGuardian = storageStudentGuardian;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(inputStudentId, inputGuardianId))
                    .ReturnsAsync(storageStudentGuardian);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentGuardianAsync(storageStudentGuardian))
                    .ReturnsAsync(expectedStudentGuardian);

            // when
            StudentGuardian actualStudentGuardian =
                await this.studentGuardianService.RemoveStudentGuardianByIdsAsync(inputStudentId, inputGuardianId);

            // then
            actualStudentGuardian.Should().BeEquivalentTo(expectedStudentGuardian);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(inputStudentId, inputGuardianId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentGuardianAsync(storageStudentGuardian),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
