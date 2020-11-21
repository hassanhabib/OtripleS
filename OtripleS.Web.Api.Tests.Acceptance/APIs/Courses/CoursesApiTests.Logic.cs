// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Courses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Courses
{
    public partial class CoursesApiTests
    {
        [Fact]
        public async Task ShouldPostCourseAsync()
        {
            // given
            Course randomCourse = CreateRandomCourse();
            Course inputCourse = randomCourse;
            Course expectedCourse = inputCourse;

            // when 
            await this.otripleSApiBroker.PostCourseAsync(inputCourse);

            Course actualCourse =
                await this.otripleSApiBroker.GetCourseByIdAsync(inputCourse.Id);

            // then
            actualCourse.Should().BeEquivalentTo(expectedCourse);

            await this.otripleSApiBroker.DeleteCourseByIdAsync(actualCourse.Id);
        }

        [Fact]
        public async Task ShouldPutCourseAsync()
        {
            // given
            Course randomCourse = CreateRandomCourse();
            await this.otripleSApiBroker.PostCourseAsync(randomCourse);
            Course modifiedCourse = UpdateCourseRandom(randomCourse);

            // when
            await this.otripleSApiBroker.PutCourseAsync(modifiedCourse);

            Course actualCourse =
                await this.otripleSApiBroker.GetCourseByIdAsync(randomCourse.Id);

            // then
            actualCourse.Should().BeEquivalentTo(modifiedCourse);

            await this.otripleSApiBroker.DeleteCourseByIdAsync(actualCourse.Id);
        }

        [Fact]
        public async Task ShouldGetAllCoursesAsync()
        {
            // given
            IEnumerable<Course> randomCourses = GetRandomCourses();
            IEnumerable<Course> inputCourses = randomCourses;

            foreach (Course course in inputCourses)
            {
                await this.otripleSApiBroker.PostCourseAsync(course);
            }

            List<Course> expectedCourses = inputCourses.ToList();

            // when
            List<Course> actualCourses = await this.otripleSApiBroker.GetAllCoursesAsync();

            // then
            foreach (Course expectedCourse in expectedCourses)
            {
                Course actualCourse = actualCourses.Single(course => course.Id == expectedCourse.Id);
                actualCourse.Should().BeEquivalentTo(expectedCourse);
                await this.otripleSApiBroker.DeleteCourseByIdAsync(actualCourse.Id);
            }
        }
    }
}
