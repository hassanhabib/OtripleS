// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using OtripleS.Web.Api.Models.SemesterCourses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.SemesterCourses
{
    public partial class SemesterCoursesApiTests
    {
        [Fact]
        public async Task ShouldPostSemesterCourseAsync()
        {
            // given
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse();
            SemesterCourse inputSemesterCourse = randomSemesterCourse;

            SemesterCourse expectedSemesterCourse =
                CreateExpectedSemesterCourse(inputSemesterCourse);

            // when 
            await this.otripleSApiBroker.PostSemesterCourseAsync(inputSemesterCourse);

            SemesterCourse actualSemesterCourse =
                await this.otripleSApiBroker.GetSemesterCourseByIdAsync(inputSemesterCourse.Id);

            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            await this.otripleSApiBroker.DeleteSemesterCourseByIdAsync(actualSemesterCourse.Id);
        }

        [Fact]
        public async Task ShouldPutSemesterCourseAsync()
        {
            // given
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse();
            await this.otripleSApiBroker.PostSemesterCourseAsync(randomSemesterCourse);
            SemesterCourse modifiedSemesterCourse = UpdateSemesterCourseRandom(randomSemesterCourse);

            // when
            await this.otripleSApiBroker.PutSemesterCourseAsync(modifiedSemesterCourse);

            SemesterCourse expectedSemesterCourse =
                CreateExpectedSemesterCourse(modifiedSemesterCourse);

            SemesterCourse actualSemesterCourse =
                await this.otripleSApiBroker.GetSemesterCourseByIdAsync(randomSemesterCourse.Id);

            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            await this.otripleSApiBroker.DeleteSemesterCourseByIdAsync(actualSemesterCourse.Id);
        }

        [Fact]
        public async Task ShouldGetAllSemesterCoursesAsync()
        {
            // given
            IEnumerable<SemesterCourse> randomSemesterCourses = CreateRandomSemesterCourses();
            List<SemesterCourse> inputSemesterCourses = randomSemesterCourses.ToList();

            foreach (SemesterCourse semesterCourse in inputSemesterCourses)
            {
                await this.otripleSApiBroker.PostSemesterCourseAsync(semesterCourse);
            }

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
                await this.otripleSApiBroker.DeleteSemesterCourseByIdAsync(actualSemesterCourse.Id);
            }
        }
    }
}
