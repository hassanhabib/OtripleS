using FluentAssertions;

using Moq;

using OtripleS.Web.Api.Models.Courses;

using System;
using System.Threading.Tasks;

using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseServiceTests
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async Task ShouldCreateCourseAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Course randomCourse = CreateRandomCourse(dateTime);
            randomCourse.UpdatedBy = randomCourse.CreatedBy;
            randomCourse.UpdatedDate = randomCourse.CreatedDate;
            Course inputCourse = randomCourse;
            Course storageCourse = randomCourse;
            Course expectedCourse = randomCourse;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCourseAsync(inputCourse))
                    .ReturnsAsync(storageCourse);

            // when
            Course actualCourse =
                await this.courseService.CreateCourseAsync(inputCourse);

            // then
            actualCourse.Should().BeEquivalentTo(expectedCourse);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAsync(inputCourse),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
