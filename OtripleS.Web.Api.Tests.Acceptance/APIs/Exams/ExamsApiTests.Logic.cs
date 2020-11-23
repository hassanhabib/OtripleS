// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Tests.Models.Exams;
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
    }
}
