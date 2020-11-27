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
using OtripleS.Web.Api.Tests.Acceptance.Models.Guardians;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentGuardians;
using OtripleS.Web.Api.Tests.Acceptance.Models.Students;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentGuardians
{
    public partial class StudentGuardiansApiTests
    {
        [Fact]
        public async Task ShouldPostStudentGuardianAsync()
        {
            // given
            StudentGuardian randomStudentGuardian = await CreateRandomStudentGuardian();
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            StudentGuardian expectedStudentGuardian = inputStudentGuardian;

            // when             
            StudentGuardian persistedStudentGuardian =
                await this.otripleSApiBroker.PostStudentGuardianAsync(inputStudentGuardian);

            StudentGuardian actualStudentGuardian =
                await this.otripleSApiBroker.GetStudentGuardianByIdsAsync(
                    persistedStudentGuardian.StudentId,
                    persistedStudentGuardian.GuardianId);

            // then
            actualStudentGuardian.Should().BeEquivalentTo(expectedStudentGuardian);
            await DeleteStudentGuardianAsync(actualStudentGuardian);
        }

        [Fact]
        public async Task ShouldPutStudentGuardianAsync()
        {
            // given
            StudentGuardian randomStudentGuardian = await PostStudentGuardianAsync();
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            StudentGuardian modifiedStudentGuardian = randomStudentGuardian.DeepClone();
            modifiedStudentGuardian.UpdatedDate = DateTimeOffset.UtcNow;

            // when
            await this.otripleSApiBroker.PutStudentGuardianAsync(modifiedStudentGuardian);

            StudentGuardian actualStudentGuardian =
                await this.otripleSApiBroker.GetStudentGuardianByIdsAsync(
                    randomStudentGuardian.StudentId,
                    randomStudentGuardian.GuardianId);

            // then
            actualStudentGuardian.Should().BeEquivalentTo(modifiedStudentGuardian);
            await DeleteStudentGuardianAsync(actualStudentGuardian);
        }

        [Fact]
        public async Task ShouldGetAllStudentGuardiansAsync()
        {
            // given
            var randomStudentGuardians = new List<StudentGuardian>();

            for(var i =0; i<= GetRandomNumber(); i++)
            {
                StudentGuardian randomStudentGuardian = await PostStudentGuardianAsync();
                randomStudentGuardians.Add(randomStudentGuardian);
            }

            List<StudentGuardian> inputStudentGuardians = randomStudentGuardians;
            List<StudentGuardian> expectedStudentGuardians = inputStudentGuardians;

            // when
            List<StudentGuardian> actualStudentGuardians = 
                await this.otripleSApiBroker.GetAllStudentGuardiansAsync();

            // then
            foreach (StudentGuardian expectedStudentGuardian in expectedStudentGuardians)
            {
                StudentGuardian actualStudentGuardian = 
                    actualStudentGuardians.Single(studentGuardian => 
                    studentGuardian.StudentId == expectedStudentGuardian.StudentId);

                actualStudentGuardian.Should().BeEquivalentTo(expectedStudentGuardian);

                await DeleteStudentGuardianAsync(actualStudentGuardian);
            }
        }

        [Fact]
        public async Task ShouldDeleteStudentGuardianAsync()
        {
            // given
            StudentGuardian randomStudentGuardian = await PostStudentGuardianAsync();
            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            StudentGuardian expectedStudentGuardian = inputStudentGuardian;

            // when 
            StudentGuardian deletedStudentGuardian = 
                await DeleteStudentGuardianAsync(inputStudentGuardian);

            ValueTask<StudentGuardian> getStudentGuardianByIdTask =
                this.otripleSApiBroker.GetStudentGuardianByIdsAsync(
                    inputStudentGuardian.StudentId, 
                    inputStudentGuardian.GuardianId);

            // then
            deletedStudentGuardian.Should().BeEquivalentTo(expectedStudentGuardian);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getStudentGuardianByIdTask.AsTask());
        }
    }
}
