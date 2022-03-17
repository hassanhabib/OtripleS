// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.SemesterCourses
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldModifySemesterCourseAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(randomInputDate);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            SemesterCourse afterUpdateStorageSemesterCourse = inputSemesterCourse;
            SemesterCourse expectedSemesterCourse = afterUpdateStorageSemesterCourse;
            SemesterCourse beforeUpdateStorageSemesterCourse = randomSemesterCourse.DeepClone();
            inputSemesterCourse.UpdatedDate = randomDate;
            Guid semesterCourseId = inputSemesterCourse.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSemesterCourseByIdAsync(semesterCourseId))
                    .ReturnsAsync(beforeUpdateStorageSemesterCourse);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateSemesterCourseAsync(inputSemesterCourse))
                    .ReturnsAsync(afterUpdateStorageSemesterCourse);

            // when
            SemesterCourse actualSemesterCourse =
                await this.semesterCourseService.ModifySemesterCourseAsync(inputSemesterCourse);

            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(semesterCourseId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateSemesterCourseAsync(inputSemesterCourse),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
