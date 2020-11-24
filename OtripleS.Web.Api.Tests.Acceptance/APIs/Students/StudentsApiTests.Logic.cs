// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Students;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Students
{
    public partial class StudentsApiTests
    {
        [Fact]
        public async Task ShouldPostStudentAsync()
        {
            // given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student expectedStudent = inputStudent;

            // when 
            await this.otripleSApiBroker.PostStudentAsync(inputStudent);

            Student actualStudent =
                await this.otripleSApiBroker.GetStudentByIdAsync(inputStudent.Id);

            // then
            actualStudent.Should().BeEquivalentTo(expectedStudent);
            await this.otripleSApiBroker.DeleteStudentByIdAsync(actualStudent.Id);
        }

        [Fact]
        public async Task ShouldPutStudentAsync()
        {
            // given
            Student randomStudent = await PostRandomStudentAsync();
            Student modifiedStudent = UpdateStudentRandom(randomStudent);

            // when
            await this.otripleSApiBroker.PutStudentAsync(modifiedStudent);

            Student actualStudent =
                await this.otripleSApiBroker.GetStudentByIdAsync(randomStudent.Id);

            // then
            actualStudent.Should().BeEquivalentTo(modifiedStudent);
            await this.otripleSApiBroker.DeleteStudentByIdAsync(actualStudent.Id);
        }

        [Fact]
        public async Task ShouldGetAllStudentsAsync()
        {
            // given
            IEnumerable<Student> randomStudents = GetRandomStudents();
            IEnumerable<Student> inputStudents = randomStudents;

            foreach (Student student in inputStudents)
            {
                await this.otripleSApiBroker.PostStudentAsync(student);
            }

            List<Student> expectedStudents = inputStudents.ToList();

            // when
            List<Student> actualStudents = await this.otripleSApiBroker.GetAllStudentsAsync();

            // then
            foreach (Student expectedStudent in expectedStudents)
            {
                Student actualStudent = actualStudents.Single(student => student.Id == expectedStudent.Id);
                actualStudent.Should().BeEquivalentTo(expectedStudent);
                await this.otripleSApiBroker.DeleteStudentByIdAsync(actualStudent.Id);
            }
        }

        [Fact]
        public async Task ShouldDeleteStudentAsync()
        {
            // given
            Student randomStudent = await PostRandomStudentAsync();
            Student inputStudent = randomStudent;
            Student expectedStudent = inputStudent;

            // when 
            Student deletedStudent = 
                await this.otripleSApiBroker.DeleteStudentByIdAsync(inputStudent.Id);

            ValueTask<Student> getStudentByIdTask =
                this.otripleSApiBroker.GetStudentByIdAsync(inputStudent.Id);

            // then
            deletedStudent.Should().BeEquivalentTo(expectedStudent);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getStudentByIdTask.AsTask());
        }
    }
}
