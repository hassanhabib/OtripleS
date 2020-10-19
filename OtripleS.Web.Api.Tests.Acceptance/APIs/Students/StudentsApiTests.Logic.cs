// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Models.Students;
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
            Student randomStudent = CreateRandomStudent();
            await this.otripleSApiBroker.PostStudentAsync(randomStudent);
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
    }
}
