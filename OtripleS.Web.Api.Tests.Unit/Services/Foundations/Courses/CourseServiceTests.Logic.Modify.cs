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
        public async Task ShouldModifyCourseAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(randomInputDate);
            Course inputCourse = randomCourse;
            Course afterUpdateStorageCourse = inputCourse;
            Course expectedCourse = afterUpdateStorageCourse;
            Course beforeUpdateStorageCourse = randomCourse.DeepClone();
            inputCourse.UpdatedDate = randomDate;
            Guid courseId = inputCourse.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(courseId))
                    .ReturnsAsync(beforeUpdateStorageCourse);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateCourseAsync(inputCourse))
                    .ReturnsAsync(afterUpdateStorageCourse);

            // when
            Course actualCourse =
                await this.courseService.ModifyCourseAsync(inputCourse);

            // then
            actualCourse.Should().BeEquivalentTo(expectedCourse);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(courseId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCourseAsync(inputCourse),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
