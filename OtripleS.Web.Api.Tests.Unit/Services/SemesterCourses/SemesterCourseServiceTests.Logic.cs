// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.SemesterCourses
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldCreateSemesterCourseAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(randomDateTime);
            randomSemesterCourse.UpdatedBy = randomSemesterCourse.CreatedBy;
            randomSemesterCourse.UpdatedDate = randomSemesterCourse.CreatedDate;
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            SemesterCourse storageSemesterCourse = randomSemesterCourse;
            SemesterCourse expectedSemesterCourse = storageSemesterCourse;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSemesterCourseAsync(inputSemesterCourse))
                    .ReturnsAsync(storageSemesterCourse);

            // when
            SemesterCourse actualSemesterCourse =
                await this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSemesterCourseAsync(inputSemesterCourse),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetireveAllSemesterCourses()
        {
            // given
            IQueryable<SemesterCourse> randomSemesterCourses =
                CreateRandomSemesterCourses();

            IQueryable<SemesterCourse> storageSemesterCourses = randomSemesterCourses;
            IQueryable<SemesterCourse> expectedSemesterCourses = storageSemesterCourses;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSemesterCourses())
                    .Returns(storageSemesterCourses);

            // when
            IQueryable<SemesterCourse> actualSemesterCourses =
                this.semesterCourseService.RetrieveAllSemesterCourses();

            // then
            actualSemesterCourses.Should().BeEquivalentTo(expectedSemesterCourses);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSemesterCourses(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldDeleteSemesterCourseAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            Guid inputSemesterCourseId = randomSemesterCourse.Id;
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            SemesterCourse storageSemesterCourse = inputSemesterCourse;
            SemesterCourse expectedSemesterCourse = storageSemesterCourse;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId))
                    .ReturnsAsync(inputSemesterCourse);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteSemesterCourseAsync(inputSemesterCourse))
                    .ReturnsAsync(storageSemesterCourse);

            // when
            SemesterCourse actualSemesterCourse =
                await this.semesterCourseService.DeleteSemesterCourseAsync(inputSemesterCourseId);

            //then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSemesterCourseAsync(inputSemesterCourse),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveSemesterCourseByIdAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(randomDateTime);
            SemesterCourse storageSemesterCourse = randomSemesterCourse;
            SemesterCourse expectedSemesterCourse = storageSemesterCourse;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId))
                    .ReturnsAsync(storageSemesterCourse);

            // when
            SemesterCourse actualSemesterCourse =
                await this.semesterCourseService.RetrieveSemesterCourseByIdAsync(inputSemesterCourseId);

            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();

        }

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
