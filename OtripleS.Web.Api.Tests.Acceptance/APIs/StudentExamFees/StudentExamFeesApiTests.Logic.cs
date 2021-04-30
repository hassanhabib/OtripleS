// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentExamFees;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentExamFees
{
    public partial class StudentExamFeesApiTests
    {
        [Fact]
        public async Task ShouldPostStudentExamFeeAsync()
        {
            // given
            StudentExamFee randomStudentExamFee = await CreateRandomStudentExamFeeAsync();
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            StudentExamFee expectedStudentExamFee = inputStudentExamFee;

            // when 
            await this.otripleSApiBroker.PostStudentExamFeeAsync(inputStudentExamFee);

            StudentExamFee actualStudentExamFee =
               await this.otripleSApiBroker.GetStudentExamFeeByIdsAsync(
                   inputStudentExamFee.StudentId,
                   inputStudentExamFee.ExamFeeId);

            // then
            actualStudentExamFee.Should().BeEquivalentTo(expectedStudentExamFee);
            await DeleteStudentExamFeeAsync(actualStudentExamFee);
        }

        [Fact]
        public async Task ShouldPutStudentExamFeeAsync()
        {
            // given
            StudentExamFee randomStudentExamFee = await PostRandomStudentExamFeeAsync();
            StudentExamFee modifiedStudentExamFee = await UpdateStudentExamFeeRandom(randomStudentExamFee);

            // when
            await this.otripleSApiBroker.PutStudentExamFeeAsync(modifiedStudentExamFee);

            StudentExamFee actualStudentExamFee =
                await this.otripleSApiBroker.GetStudentExamFeeByIdsAsync(
                   randomStudentExamFee.StudentId,
                   randomStudentExamFee.ExamFeeId);

            // then
            actualStudentExamFee.Should().BeEquivalentTo(modifiedStudentExamFee);
            await DeleteStudentExamFeeAsync(actualStudentExamFee);
        }

        [Fact]
        public async Task ShouldGetAllStudentExamFeesAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            var randomStudentExamFees = new List<StudentExamFee>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomStudentExamFees.Add(await PostRandomStudentExamFeeAsync());
            }

            List<StudentExamFee> inputStudentExamFees = randomStudentExamFees;
            List<StudentExamFee> expectedStudentExamFees = inputStudentExamFees.ToList();

            // when
            List<StudentExamFee> actualStudentExamFees = await this.otripleSApiBroker.GetAllStudentExamFeesAsync();

            // then
            foreach (StudentExamFee expectedStudentExamFee in expectedStudentExamFees)
            {
                StudentExamFee actualStudentExamFee = actualStudentExamFees.Single(
                    studentExamFee => studentExamFee.StudentId == expectedStudentExamFee.StudentId &&
                                      studentExamFee.ExamFeeId == expectedStudentExamFee.ExamFeeId);

                actualStudentExamFee.Should().BeEquivalentTo(expectedStudentExamFee);
                await DeleteStudentExamFeeAsync(actualStudentExamFee);
            }
        }

        [Fact]
        public async Task ShouldDeleteStudentExamFeeAsync()
        {
            // given
            StudentExamFee randomStudentExamFee = await PostRandomStudentExamFeeAsync();
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            StudentExamFee expectedStudentExamFee = inputStudentExamFee;

            // when 
            StudentExamFee deletedStudentExamFee = await DeleteStudentExamFeeAsync(inputStudentExamFee);

            ValueTask<StudentExamFee> getStudentExamFeeByIdTask =
                this.otripleSApiBroker.GetStudentExamFeeByIdsAsync(
                   randomStudentExamFee.StudentId,
                   randomStudentExamFee.ExamFeeId);

            // then
            deletedStudentExamFee.Should().BeEquivalentTo(expectedStudentExamFee);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getStudentExamFeeByIdTask.AsTask());
        }
    }
}
