// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentRegistrations;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentRegistrations
{
    public partial class StudentRegistrationsApiTests
    {
        [Fact]
        public async Task ShouldPostStudentRegistrationAsync()
        {
            // given
            StudentRegistration randomStudentRegistration = await CreateRandomStudentRegistration();
            StudentRegistration inputStudentRegistration = randomStudentRegistration;
            StudentRegistration expectedStudentRegistration = inputStudentRegistration;

            // when             
            await this.otripleSApiBroker.PostStudentRegistrationAsync(inputStudentRegistration);

            StudentRegistration actualStudentRegistration =
                await this.otripleSApiBroker.GetStudentRegistrationByIdsAsync(
                    inputStudentRegistration.StudentId,
                    inputStudentRegistration.RegistrationId);

            // then
            actualStudentRegistration.Should().BeEquivalentTo(expectedStudentRegistration);
            await DeleteStudentRegistrationAsync(actualStudentRegistration);
        }

        
    }
}
