// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Teachers;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Teachers
{
    public partial class TeacherServiceTests
    {
        [Fact]
        public async Task ShouldCreateTeacherAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            randomTeacher.UpdatedBy = randomTeacher.CreatedBy;
            randomTeacher.UpdatedDate = randomTeacher.CreatedDate;
            Teacher inputTeacher = randomTeacher;
            Teacher storageTeacher = randomTeacher;
            Teacher expectedTeacher = randomTeacher;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherAsync(inputTeacher))
                    .ReturnsAsync(storageTeacher);

            // when
            Teacher actualTeacher =
                await this.teacherService.CreateTeacherAsync(inputTeacher);

            // then
            actualTeacher.Should().BeEquivalentTo(expectedTeacher);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherAsync(inputTeacher),
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
