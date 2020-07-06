// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
    }
}
