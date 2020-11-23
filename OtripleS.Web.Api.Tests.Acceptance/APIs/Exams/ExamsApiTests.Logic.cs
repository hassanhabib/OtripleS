// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Exams;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Exams
{
    public partial class ExamsApiTests
    {
        [Fact]
        public async Task ShouldPostExamAsync()
        {
            // given
            Exam randomExam = await CreateRandomExamAsync();
            Exam inputExam = randomExam;
            Exam expectedExam = inputExam;

            // when 
            Exam actualExam =
                await this.otripleSApiBroker.PostExamAsync(inputExam);

            // then
            actualExam.Should().BeEquivalentTo(expectedExam);
            await DeleteExamAsync(actualExam);
        }

        [Fact]
        public async Task ShouldPutExamAsync()
        {
            // given
            Exam randomExam = await PostRandomExamAsync();
            Exam modifiedExam = await UpdateExamRandom(randomExam);

            // when
            await this.otripleSApiBroker.PutExamAsync(modifiedExam);

            Exam actualExam =
                await this.otripleSApiBroker.GetExamByIdAsync(randomExam.Id);

            // then
            actualExam.Should().BeEquivalentTo(modifiedExam);
            await DeleteExamAsync(actualExam);
        }

        [Fact]
        public async Task ShouldGetAllExamsAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            var randomExams = new List<Exam>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomExams.Add(await PostRandomExamAsync());
            }

            List<Exam> inputExams = randomExams;
            List<Exam> expectedExams = inputExams.ToList();

            // when
            List<Exam> actualExams = await this.otripleSApiBroker.GetAllExamsAsync();

            // then
            foreach (Exam expectedExam in expectedExams)
            {
                Exam actualExam = actualExams.Single(student => student.Id == expectedExam.Id);
                actualExam.Should().BeEquivalentTo(expectedExam);
                await DeleteExamAsync(actualExam);
            }
        }

        [Fact]
        public async Task ShouldDeleteExamAsync()
        {
            // given
            Exam randomExam = await PostRandomExamAsync();
            Exam inputExam = randomExam;
            Exam expectedExam = inputExam;

            // when 
            Exam deletedExam = await DeleteExamAsync(inputExam);

            ValueTask<Exam> getExamByIdTask =
                this.otripleSApiBroker.GetExamByIdAsync(inputExam.Id);

            // then
            deletedExam.Should().BeEquivalentTo(expectedExam);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getExamByIdTask.AsTask());
        }
    }
}
