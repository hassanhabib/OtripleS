// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentGuardians
{
    public partial class StudentGuardianServiceTests
    {
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
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
