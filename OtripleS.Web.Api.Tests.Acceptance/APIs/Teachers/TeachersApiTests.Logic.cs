// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Teachers
{
    public partial class TeachersApiTests
    {
        [Fact]
        public async Task ShouldPostTeacherAsync()
        {
            // given
            Teacher randomTeacher = CreateRandomTeacher();
            Teacher inputTeacher = randomTeacher;
            Teacher expectedTeacher = inputTeacher;

            // when 
            await this.otripleSApiBroker.PostTeacherAsync(inputTeacher);

            Teacher actualTeacher =
                await this.otripleSApiBroker.GetTeacherByIdAsync(inputTeacher.Id);

            // then
            actualTeacher.Should().BeEquivalentTo(expectedTeacher);
            await this.otripleSApiBroker.DeleteTeacherByIdAsync(actualTeacher.Id);
        }

        [Fact]
        public async Task ShouldPutTeacherAsync()
        {
            // given
            Teacher randomTeacher = await PostRandomTeacherAsync();
            Teacher modifiedTeacher = UpdateTeacherRandom(randomTeacher);

            // when
            await this.otripleSApiBroker.PutTeacherAsync(modifiedTeacher);

            Teacher actualTeacher =
                await this.otripleSApiBroker.GetTeacherByIdAsync(randomTeacher.Id);

            // then
            actualTeacher.Should().BeEquivalentTo(modifiedTeacher);
            await this.otripleSApiBroker.DeleteTeacherByIdAsync(actualTeacher.Id);
        }

        [Fact]
        public async Task ShouldGetAllTeachersAsync()
        {
            // given
            IEnumerable<Teacher> randomTeachers = CreateRandomTeachers();
            IEnumerable<Teacher> inputTeachers = randomTeachers;
            IEnumerable<Teacher> expectedTeachers = inputTeachers;

            foreach (Teacher inputTeacher in inputTeachers)
            {
                await this.otripleSApiBroker.PostTeacherAsync(inputTeacher);
            }

            // when
            IEnumerable<Teacher> actualTeachers =
                await this.otripleSApiBroker.GetAllTeachersAsync();

            // then

            foreach (Teacher expectedTeacher in expectedTeachers)
            {
                Teacher actualTeacher =
                    actualTeachers.Single(teacher => teacher.Id == expectedTeacher.Id);

                actualTeacher.Should().BeEquivalentTo(expectedTeacher);
                await this.otripleSApiBroker.DeleteTeacherByIdAsync(actualTeacher.Id);
            }
        }

        [Fact]
        public async Task ShouldDeleteTeacherAsync()
        {
            // given
            Teacher randomTeacher = await PostRandomTeacherAsync();
            Teacher inputTeacher = randomTeacher;
            Teacher expectedTeacher = inputTeacher;

            // when 
            Teacher deletedTeacher = await this.otripleSApiBroker.DeleteTeacherByIdAsync(inputTeacher.Id);

            ValueTask<Teacher> getTeacherByIdTask =
                this.otripleSApiBroker.GetTeacherByIdAsync(inputTeacher.Id);

            // then
            deletedTeacher.Should().BeEquivalentTo(expectedTeacher);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getTeacherByIdTask.AsTask());
        }
    }
}
