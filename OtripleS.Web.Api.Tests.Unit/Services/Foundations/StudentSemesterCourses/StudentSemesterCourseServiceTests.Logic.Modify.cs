// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentSemesterCourses
{
    public partial class StudentSemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldModifyStudentSemesterCourseAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(randomInputDate);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            StudentSemesterCourse afterUpdateStorageStudentSemesterCourse = inputStudentSemesterCourse;
            StudentSemesterCourse expectedStudentSemesterCourse = afterUpdateStorageStudentSemesterCourse;
            StudentSemesterCourse beforeUpdateStorageStudentSemesterCourse = randomStudentSemesterCourse.DeepClone();
            inputStudentSemesterCourse.UpdatedDate = randomDate;
            Guid studentId = inputStudentSemesterCourse.StudentId;
            Guid semesterCourseId = inputStudentSemesterCourse.SemesterCourseId;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(studentId, semesterCourseId))
                    .ReturnsAsync(beforeUpdateStorageStudentSemesterCourse);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateStudentSemesterCourseAsync(inputStudentSemesterCourse))
                    .ReturnsAsync(afterUpdateStorageStudentSemesterCourse);

            // when
            StudentSemesterCourse actualStudentSemesterCourse =
                await this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            actualStudentSemesterCourse.Should().BeEquivalentTo(expectedStudentSemesterCourse);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(studentId, semesterCourseId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentSemesterCourseAsync(inputStudentSemesterCourse),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
