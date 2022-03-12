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
        public async Task ShouldRetrieveStudentGuardianById()
        {
            // given
            DateTimeOffset inputDateTime = GetRandomDateTime();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(inputDateTime);
            StudentGuardian storageStudentGuardian = randomStudentGuardian;
            StudentGuardian expectedStudentGuardian = storageStudentGuardian;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(randomStudentGuardian.StudentId, randomStudentGuardian.GuardianId))
                    .ReturnsAsync(randomStudentGuardian);

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
    }
}
