// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Courses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Courses
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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAsync(inputCourse),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
