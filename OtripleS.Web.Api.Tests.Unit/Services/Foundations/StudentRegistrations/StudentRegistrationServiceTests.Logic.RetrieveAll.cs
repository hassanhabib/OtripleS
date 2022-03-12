// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentRegistrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentRegistrations
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
        }
    }
}
