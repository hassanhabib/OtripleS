// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.ExamFees;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.ExamFees
{
    public partial class ExamFeesApiTests
    {
        [Fact]
        public async Task ShouldPostExamFeeAsync()
        {
            // given
            ExamFee randomExamFee = await CreateRandomExamFeeAsync();
            ExamFee inputExamFee = randomExamFee;
            ExamFee expectedExamFee = inputExamFee;

            // when 
            await this.otripleSApiBroker.PostExamFeeAsync(inputExamFee);

            ExamFee actualExamFee =
               await this.otripleSApiBroker.GetExamFeeByIdAsync(inputExamFee.Id);

            // then
            actualExamFee.Should().BeEquivalentTo(expectedExamFee);
            await DeleteExamFeeAsync(actualExamFee);
        }

        
    }
}
