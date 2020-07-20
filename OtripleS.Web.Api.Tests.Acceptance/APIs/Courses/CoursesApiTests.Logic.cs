// --------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// --------------------------------------------------------------
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Models.Courses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Courses
{
    public partial class CoursesApiTests
    {
        [Fact]
        public async Task ShouldPostCourseAsync()
        {
            // given
            Course randomCourse = CreateRandumCourse();
            Course inputCourse = randomCourse;
            Course expectedCourse = inputCourse;

            // when 
            await this.courseBroker.PostCourseAsync(inputCourse);

            Course actualCourse =
                await this.courseBroker.GetCourseByIdAsync(inputCourse.Id);

            // then
            actualCourse.Should().BeEquivalentTo(expectedCourse);

            await this.courseBroker.DeleteCourseByIdAsync(actualCourse.Id);
        }
    }
}
