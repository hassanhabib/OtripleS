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

        [Fact]
        public async Task ShouldGetAllStudentRegistrationsAsync()
        {
            // given
            var randomStudentRegistrations = new List<StudentRegistration>();

            for (var i = 0; i <= GetRandomNumber(); i++)
            {
                StudentRegistration randomStudentRegistration = await PostStudentRegistrationAsync();
                randomStudentRegistrations.Add(randomStudentRegistration);
            }

            List<StudentRegistration> inputStudentRegistrations = randomStudentRegistrations;
            List<StudentRegistration> expectedStudentRegistrations = inputStudentRegistrations;

            // when
            List<StudentRegistration> actualStudentRegistrations =
                await this.otripleSApiBroker.GetAllStudentRegistrationsAsync();

            // then
            foreach (StudentRegistration expectedStudentRegistration in expectedStudentRegistrations)
            {
                StudentRegistration actualStudentRegistration =
                    actualStudentRegistrations.Single(studentRegistration =>
                    studentRegistration.StudentId == expectedStudentRegistration.StudentId);

                actualStudentRegistration.Should().BeEquivalentTo(expectedStudentRegistration);

                await DeleteStudentRegistrationAsync(actualStudentRegistration);
            }
        }

        [Fact]
        public async Task ShouldDeleteStudentRegistrationAsync()
        {
            // given
            StudentRegistration randomStudentRegistration = await PostStudentRegistrationAsync();
            StudentRegistration inputStudentRegistration = randomStudentRegistration;
            StudentRegistration expectedStudentRegistration = inputStudentRegistration;

            // when 
            StudentRegistration deletedStudentRegistration =
                await DeleteStudentRegistrationAsync(inputStudentRegistration);

            ValueTask<StudentRegistration> getStudentRegistrationByIdTask =
                this.otripleSApiBroker.GetStudentRegistrationByIdsAsync(
                    inputStudentRegistration.StudentId,
                    inputStudentRegistration.RegistrationId);

            // then
            deletedStudentRegistration.Should().BeEquivalentTo(expectedStudentRegistration);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getStudentRegistrationByIdTask.AsTask());
        }
    }
}
