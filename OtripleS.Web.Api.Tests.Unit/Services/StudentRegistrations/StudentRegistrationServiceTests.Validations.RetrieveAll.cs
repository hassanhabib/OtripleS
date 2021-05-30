// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentRegistrations;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenStudentRegistrationsWasEmptyAndLogIt()
        {
            // given
            IQueryable<StudentRegistration> emptyStorageStudentRegistrations = new List<StudentRegistration>().AsQueryable();
            IQueryable<StudentRegistration> expectedStudentRegistrations = emptyStorageStudentRegistrations;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentRegistrations())
                    .Returns(expectedStudentRegistrations);

            // when
            IQueryable<StudentRegistration> actualStudentRegistrations =
                this.studentRegistrationService.RetrieveAllStudentRegistrations();

            // then
            actualStudentRegistrations.Should().BeEquivalentTo(emptyStorageStudentRegistrations);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No studentRegistrations found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentRegistrations(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
