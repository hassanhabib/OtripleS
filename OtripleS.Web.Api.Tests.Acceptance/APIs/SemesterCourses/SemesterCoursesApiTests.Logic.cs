// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.SemesterCourses;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.SemesterCourses
{
    public partial class SemesterCoursesApiTests
    {
        [Fact]
        public async Task ShouldPostSemesterCourseAsync()
        {
            // given
            SemesterCourse randomSemesterCourse = await CreateRandomSemesterCourseAsync();
            SemesterCourse inputSemesterCourse = randomSemesterCourse;

            SemesterCourse expectedSemesterCourse =
                CreateExpectedSemesterCourse(inputSemesterCourse);

            // when 
            await this.otripleSApiBroker.PostSemesterCourseAsync(inputSemesterCourse);

            SemesterCourse actualSemesterCourse =
                await this.otripleSApiBroker.GetSemesterCourseByIdAsync(inputSemesterCourse.Id);

            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);
            await DeleteSemesterCourseAsync(actualSemesterCourse);
        }

        [Fact]
        public async Task ShouldPutSemesterCourseAsync()
        {
            // given
            SemesterCourse randomSemesterCourse = await PostRandomSemesterCourseAsync();
            SemesterCourse modifiedSemesterCourse = UpdateSemesterCourseRandom(randomSemesterCourse);

            // when
            await this.otripleSApiBroker.PutSemesterCourseAsync(modifiedSemesterCourse);

            SemesterCourse expectedSemesterCourse =
               CreateExpectedSemesterCourse(modifiedSemesterCourse);

            SemesterCourse actualSemesterCourse =
                await this.otripleSApiBroker.GetSemesterCourseByIdAsync(randomSemesterCourse.Id);
            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);
            await DeleteSemesterCourseAsync(actualSemesterCourse);
        }

        [Fact]
        public async Task ShouldGetAllSemesterCoursesAsync()
        {
            // given
            var randomSemesterCourses = new List<SemesterCourse>();

            for (int i = 0; i <= GetRandomNumber(); i++)
            {
                randomSemesterCourses.Add(await PostRandomSemesterCourseAsync());
            }

            List<SemesterCourse> inputSemesterCourses = randomSemesterCourses.ToList();
            List<SemesterCourse> expectedSemesterCourses = inputSemesterCourses.ToList();

            // when
            List<SemesterCourse> actualSemesterCourses =
                await this.otripleSApiBroker.GetAllSemesterCoursesAsync();

            // then
            foreach (SemesterCourse expectedSemesterCourse in expectedSemesterCourses)
            {
                SemesterCourse actualSemesterCourse =
                    actualSemesterCourses.Single(semesterCourse =>
                        semesterCourse.Id == expectedSemesterCourse.Id);

                SemesterCourse expectedReturnedCourse = CreateExpectedSemesterCourse(expectedSemesterCourse);
                actualSemesterCourse.Should().BeEquivalentTo(expectedReturnedCourse);
                await DeleteSemesterCourseAsync(actualSemesterCourse);
            }
        }

        [Fact]
        public async Task ShouldDeleteSemesterCourseAsync()
        {
            // given
            SemesterCourse randomSemesterCourse = await PostRandomSemesterCourseAsync();
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            SemesterCourse expectedSemesterCourse = inputSemesterCourse;

            // when 
            SemesterCourse deletedSemesterCourse =
                await DeleteSemesterCourseAsync(inputSemesterCourse);

            ValueTask<SemesterCourse> getSemesterCourseByIdTask =
                this.otripleSApiBroker.GetSemesterCourseByIdAsync(inputSemesterCourse.Id);

            // then
            deletedSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getSemesterCourseByIdTask.AsTask());
        }
    }
}
