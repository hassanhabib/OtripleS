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
    }
}
