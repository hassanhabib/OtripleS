// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentExams;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentExams
{
    public partial class StudentExamsApiTests
    {
        [Fact]
        public async Task ShouldPostStudentExamAsync()
        {
            // given
            StudentExam randomStudentExam = await CreateRandomStudentExamAsync();
            StudentExam inputStudentExam = randomStudentExam;
            StudentExam expectedStudentExam = inputStudentExam;

            // when 
            StudentExam actualStudentExam =
                await this.otripleSApiBroker.PostStudentExamAsync(inputStudentExam);

            // then
            actualStudentExam.Should().BeEquivalentTo(expectedStudentExam);
            await DeleteStudentExam(actualStudentExam);
        }

        [Fact]
        public async Task ShouldPutStudentExamAsync()
        {
            // given
            StudentExam randomStudentExam = await PostRandomStudentExamAsync();
            StudentExam modifiedStudentExam = await UpdateStudentExamRandomAsync(randomStudentExam);

            // when
            await this.otripleSApiBroker.PutStudentExamAsync(modifiedStudentExam);

            StudentExam actualStudentExam =
                await this.otripleSApiBroker.GetStudentExamByIdAsync(randomStudentExam.Id);

            // then
            actualStudentExam.Should().BeEquivalentTo(modifiedStudentExam);
            await DeleteStudentExam(actualStudentExam);
        }

        [Fact]
        public async Task ShouldGetAllStudentExamsAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            var randomStudentExams = new List<StudentExam>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomStudentExams.Add(await PostRandomStudentExamAsync());
            }

            List<StudentExam> inputStudentExams = randomStudentExams;
            List<StudentExam> expectedStudentExams = inputStudentExams.ToList();

            // when
            List<StudentExam> actualStudentExams = await this.otripleSApiBroker.GetAllStudentExamsAsync();

            // then
            foreach (StudentExam expectedStudentExam in expectedStudentExams)
            {
                StudentExam actualStudentExam = actualStudentExams.Single(student => student.Id == expectedStudentExam.Id);
                actualStudentExam.Should().BeEquivalentTo(expectedStudentExam);
                await DeleteStudentExam(actualStudentExam);
            }
        }

        [Fact]
        public async Task ShouldDeleteStudentExamAsync()
        {
            // given
            StudentExam randomStudentExam = await PostRandomStudentExamAsync();
            StudentExam inputStudentExam = randomStudentExam;
            StudentExam expectedStudentExam = inputStudentExam;

            // when 
            StudentExam deletedStudentExam = await DeleteStudentExam(inputStudentExam);

            ValueTask<StudentExam> getStudentExamByIdTask =
                this.otripleSApiBroker.GetStudentExamByIdAsync(inputStudentExam.Id);

            // then
            deletedStudentExam.Should().BeEquivalentTo(expectedStudentExam);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getStudentExamByIdTask.AsTask());
        }
    }
}
