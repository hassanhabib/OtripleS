// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldModifyStudentGuardianAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(randomInputDate);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            StudentGuardian afterUpdateStorageStudentGuardian = inputStudentGuardian;
            StudentGuardian expectedStudentGuardian = afterUpdateStorageStudentGuardian;
            StudentGuardian beforeUpdateStorageStudentGuardian = randomStudentGuardian.DeepClone();
            inputStudentGuardian.UpdatedDate = randomDate;
            Guid studentId = inputStudentGuardian.StudentId;
            Guid guardianId = inputStudentGuardian.GuardianId;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(studentId, guardianId))
                    .ReturnsAsync(beforeUpdateStorageStudentGuardian);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateStudentGuardianAsync(inputStudentGuardian))
                    .ReturnsAsync(afterUpdateStorageStudentGuardian);

            // when
            StudentGuardian actualStudentGuardian =
                await this.studentGuardianService.ModifyStudentGuardianAsync(inputStudentGuardian);

            // then
            actualStudentGuardian.Should().BeEquivalentTo(expectedStudentGuardian);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(studentId, guardianId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentGuardianAsync(inputStudentGuardian),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
