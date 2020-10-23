// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentGuardians
{
    public partial class StudentGuardianServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenStudentGuardiansWereEmptyAndLogIt()
        {
            // given
            IQueryable<StudentGuardian> emptyStorageStudentGuardians = new List<StudentGuardian>().AsQueryable();
            IQueryable<StudentGuardian> expectedStudentGuardians = emptyStorageStudentGuardians;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentGuardians())
                    .Returns(expectedStudentGuardians);

            // when
            IQueryable<StudentGuardian> actualStudentGuardians =
                this.studentGuardianService.RetrieveAllStudentGuardians();

            // then
            actualStudentGuardians.Should().BeEquivalentTo(emptyStorageStudentGuardians);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No Student Guardians found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentGuardians(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
