// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Teachers;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Teachers
{
    public partial class TeacherServiceTests
    {
        [Fact]
        public async Task ShouldModifyTeacherAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(randomInputDate);
            Teacher inputTeacher = randomTeacher;
            Teacher afterUpdateStorageTeacher = inputTeacher;
            Teacher expectedTeacher = afterUpdateStorageTeacher;
            Teacher beforeUpdateStorageTeacher = randomTeacher.DeepClone();
            inputTeacher.UpdatedDate = randomDate;
            Guid studentId = inputTeacher.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(studentId))
                    .ReturnsAsync(beforeUpdateStorageTeacher);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTeacherAsync(inputTeacher))
                    .ReturnsAsync(afterUpdateStorageTeacher);

            // when
            Teacher actualTeacher =
                await this.teacherService.ModifyTeacherAsync(inputTeacher);

            // then
            actualTeacher.Should().BeEquivalentTo(expectedTeacher);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(studentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTeacherAsync(inputTeacher),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
