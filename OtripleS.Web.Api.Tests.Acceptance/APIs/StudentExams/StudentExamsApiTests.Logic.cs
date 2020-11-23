// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentExams;
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

    }
}
