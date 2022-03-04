﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
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
        public async Task ShouldCreateStudentSemesterCourseAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            StudentSemesterCourse randomSemesterCourse = CreateRandomStudentSemesterCourse(randomDateTime);
            randomSemesterCourse.UpdatedBy = randomSemesterCourse.CreatedBy;
            randomSemesterCourse.UpdatedDate = randomSemesterCourse.CreatedDate;
            StudentSemesterCourse inputStudentSemesterCourse = randomSemesterCourse;
            StudentSemesterCourse storageStudentSemesterCourse = randomSemesterCourse;
            StudentSemesterCourse expectedStudentSemesterCourse = storageStudentSemesterCourse;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentSemesterCourseAsync(inputStudentSemesterCourse))
                    .ReturnsAsync(storageStudentSemesterCourse);

            // when
            StudentSemesterCourse actualSemesterCourse =
                await this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedStudentSemesterCourse);

            // This is called within validation code
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentSemesterCourseAsync(inputStudentSemesterCourse),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllStudentSemesterCourses()
        {
            //given
            IQueryable<StudentSemesterCourse> randomSemesterCourses =
                CreateRandomStudentSemesterCourses();

            IQueryable<StudentSemesterCourse> storageStudentSemesterCourses = randomSemesterCourses;
            IQueryable<StudentSemesterCourse> expectedStudentSemesterCourses = storageStudentSemesterCourses;

            this.storageBrokerMock.Setup(broker => broker.SelectAllStudentSemesterCourses())
                .Returns(storageStudentSemesterCourses);

            // when
            IQueryable<StudentSemesterCourse> actualStudentSemesterCourses =
                this.studentSemesterCourseService.RetrieveAllStudentSemesterCourses();

            actualStudentSemesterCourses.Should().BeEquivalentTo(expectedStudentSemesterCourses);

            this.storageBrokerMock.Verify(broker => broker.SelectAllStudentSemesterCourses(),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }        

        [Fact]
        public async Task ShouldDeleteStudentSemesterCourseAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            Guid inputSemesterCourseId = randomStudentSemesterCourse.SemesterCourseId;
            Guid inputStudentId = randomStudentSemesterCourse.StudentId;
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            StudentSemesterCourse storageStudentSemesterCourse = inputStudentSemesterCourse;
            StudentSemesterCourse expectedStudentSemesterCourse = storageStudentSemesterCourse;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputSemesterCourseId, inputStudentId))
                    .ReturnsAsync(inputStudentSemesterCourse);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentSemesterCourseAsync(inputStudentSemesterCourse))
                    .ReturnsAsync(storageStudentSemesterCourse);

            // when
            StudentSemesterCourse actualStudentSemesterCourse =
                await this.studentSemesterCourseService.RemoveStudentSemesterCourseByIdsAsync(inputSemesterCourseId, inputStudentId);

            actualStudentSemesterCourse.Should().BeEquivalentTo(expectedStudentSemesterCourse);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputSemesterCourseId, inputStudentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentSemesterCourseAsync(inputStudentSemesterCourse),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
