// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Models.Courses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Courses
{
    public partial class CoursesApiTests
    {
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
    }
}
