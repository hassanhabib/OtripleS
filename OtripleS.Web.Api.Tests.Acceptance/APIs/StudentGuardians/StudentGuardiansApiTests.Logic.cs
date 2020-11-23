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
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentGuardians
{
    public partial class StudentGuardiansApiTests
    {
        [Fact]
        public async Task ShouldPostStudentGuardianAsync()
        {
            // given
            Student persistedStudent = await PostStudentAsync();
            Guardian persistedGuardian = await PostGuardianAsync();

            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(
                persistedStudent.Id,
                persistedGuardian.Id);

            StudentGuardian inputStudentGuardian = randomStudentGuardian;
            StudentGuardian expectedStudentGuardian = inputStudentGuardian;

            // when             
            StudentGuardian persistedStudentGuardian =
                await this.otripleSApiBroker.PostStudentGuardianAsync(inputStudentGuardian);

            StudentGuardian actualStudentGuardian =
                await this.otripleSApiBroker.GetStudentGuardianAsync(persistedStudentGuardian.StudentId, persistedStudentGuardian.GuardianId);

            // then
            actualStudentGuardian.Should().BeEquivalentTo(expectedStudentGuardian);

            await this.otripleSApiBroker.DeleteStudentGuardianAsync(actualStudentGuardian.StudentId,
                actualStudentGuardian.GuardianId);
            await this.otripleSApiBroker.DeleteGuardianByIdAsync(actualStudentGuardian.GuardianId);
            await this.otripleSApiBroker.DeleteStudentByIdAsync(actualStudentGuardian.StudentId);
        }

        [Fact]
        public async Task ShouldPutStudentGuardianAsync()
        {
            // given
            Student inputStudent = await PostStudentAsync();
            Guardian inputGuardian = await PostGuardianAsync();
            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardian(inputStudent.Id, inputGuardian.Id);
            StudentGuardian inputStudentGuardian = randomStudentGuardian;

            await this.otripleSApiBroker.PostStudentGuardianAsync(inputStudentGuardian);

            StudentGuardian modifiedStudentGuardian = randomStudentGuardian.DeepClone();
            modifiedStudentGuardian.UpdatedDate = DateTimeOffset.UtcNow;

            // when
            await this.otripleSApiBroker.PutStudentGuardianAsync(modifiedStudentGuardian);

            StudentGuardian actualStudentGuardian =
                await this.otripleSApiBroker.GetStudentGuardianAsync(
                    randomStudentGuardian.StudentId,
                    randomStudentGuardian.GuardianId);

            // then
            actualStudentGuardian.Should().BeEquivalentTo(modifiedStudentGuardian);

            await this.otripleSApiBroker.DeleteStudentGuardianAsync(actualStudentGuardian.StudentId,
                actualStudentGuardian.GuardianId);
            await this.otripleSApiBroker.DeleteGuardianByIdAsync(actualStudentGuardian.GuardianId);
            await this.otripleSApiBroker.DeleteStudentByIdAsync(actualStudentGuardian.StudentId);
        }

        [Fact]
        public async Task ShouldGetAllStudentGuardiansAsync()
        {
            // given
            IEnumerable<StudentGuardian> randomStudentGuardians = GetRandomStudentGuardians();
            IEnumerable<StudentGuardian> inputStudentGuardians = randomStudentGuardians;
            List<Student> inputStudents = new List<Student>();
            List<Guardian> inputGuardians = new List<Guardian>();

            foreach (StudentGuardian studentGuardian in inputStudentGuardians)
            {
                Student randomStudent = CreateRandomStudent();
                Student inputStudent = randomStudent;
                Guardian randomGuardian = CreateRandomGuardian();
                Guardian inputGuardian = randomGuardian;
                studentGuardian.StudentId = inputStudent.Id;
                studentGuardian.GuardianId = inputGuardian.Id;
                inputStudents.Add(inputStudent);
                inputGuardians.Add(inputGuardian);

                await this.otripleSApiBroker.PostStudentAsync(inputStudent);
                await this.otripleSApiBroker.PostGuardianAsync(inputGuardian);
                await this.otripleSApiBroker.PostStudentGuardianAsync(studentGuardian);
            }

            List<StudentGuardian> expectedStudentGuardians = inputStudentGuardians.ToList();

            // when
            List<StudentGuardian> actualStudentGuardians = await this.otripleSApiBroker.GetAllStudentGuardiansAsync();

            // then
            foreach (StudentGuardian expectedStudentGuardian in expectedStudentGuardians)
            {
                StudentGuardian actualStudentGuardian = actualStudentGuardians.Single(studentGuardian => studentGuardian.StudentId == expectedStudentGuardian.StudentId);
                actualStudentGuardian.Should().BeEquivalentTo(expectedStudentGuardian);

                await this.otripleSApiBroker.DeleteStudentGuardianAsync(actualStudentGuardian.StudentId, actualStudentGuardian.GuardianId);
                await this.otripleSApiBroker.DeleteGuardianByIdAsync(actualStudentGuardian.GuardianId);
                await this.otripleSApiBroker.DeleteStudentByIdAsync(actualStudentGuardian.StudentId);
            }
        }
    }
}
