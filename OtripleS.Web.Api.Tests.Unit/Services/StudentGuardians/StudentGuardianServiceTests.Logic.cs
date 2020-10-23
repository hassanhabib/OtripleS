// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentGuardians
{
    public partial class StudentGuardianServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllStudentGuardians()
        {
            // given
            IQueryable<StudentGuardian> randomStudentGuardians =
                CreateRandomStudentGuardians();

            IQueryable<StudentGuardian> storageStudentGuardians = randomStudentGuardians;
            IQueryable<StudentGuardian> expectedStudentGuardians = storageStudentGuardians;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentGuardians())
                    .Returns(storageStudentGuardians);

            // when
            IQueryable<StudentGuardian> actualStudentGuardians =
                this.studentGuardianService.RetrieveAllStudentGuardians();

            // then
            actualStudentGuardians.Should().BeEquivalentTo(expectedStudentGuardians);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentGuardians(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveStudentGuardianById()
        {
            // given
            DateTimeOffset inputDateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(inputDateTime);
            StudentGuardian storageStudentGuardian = randomStudentGuardian;
            StudentGuardian expectedStudentGuardian = storageStudentGuardian;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(randomStudentGuardian.StudentId, randomStudentGuardian.GuardianId))
                    .Returns(new ValueTask<StudentGuardian>(randomStudentGuardian));

            // when
            StudentGuardian actualStudentGuardian = await
                this.studentGuardianService.RetrieveStudentGuardianByIdAsync(randomStudentGuardian.StudentId, randomStudentGuardian.GuardianId);

            // then
            actualStudentGuardian.Should().BeEquivalentTo(expectedStudentGuardian);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(randomStudentGuardian.StudentId, randomStudentGuardian.GuardianId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

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

        [Fact]
        public async Task ShouldAddStudentStudentGuardianAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(randomDateTime);
            randomStudentGuardian.UpdatedBy = randomStudentGuardian.CreatedBy;
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            StudentGuardian storageStudentGuardian = randomStudentGuardian;
            StudentGuardian expectedStudentGuardian = storageStudentGuardian;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentGuardianAsync(inputStudentGuardian))
                    .ReturnsAsync(storageStudentGuardian);

            // when
            StudentGuardian actualStudentGuardian =
                await this.studentGuardianService.AddStudentGuardianAsync(inputStudentGuardian);

            // then
            actualStudentGuardian.Should().BeEquivalentTo(expectedStudentGuardian);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(inputStudentGuardian),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
