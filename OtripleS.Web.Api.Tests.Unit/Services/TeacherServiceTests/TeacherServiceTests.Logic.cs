// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Teachers;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherServiceTests
{
    public partial class TeacherServiceTests
    {
        [Fact]
        public async Task ShouldDeleteTeacherAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Guid inputTeacherId = randomTeacher.Id;
            Teacher inputTeacher = randomTeacher;
            Teacher storageTeacher = randomTeacher;
            Teacher expectedTeacher = randomTeacher;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId))
                    .ReturnsAsync(inputTeacher);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTeacherAsync(inputTeacher))
                    .ReturnsAsync(storageTeacher);

            // when
            Teacher actualTeacher = 
                await this.teacherService.DeleteTeacherByIdAsync(inputTeacherId);

            // then
            actualTeacher.Should().BeEquivalentTo(expectedTeacher);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId), 
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeacherAsync(inputTeacher),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyTeacherAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(randomDate);
            Teacher inputTeacher = randomTeacher;
            Teacher afterUpdateStorageTeacher = inputTeacher;
            Teacher expectedTeacher = afterUpdateStorageTeacher;
            Teacher beforeUpdateStorageTeacher = randomTeacher.DeepClone();
            inputTeacher.UpdatedDate = randomDate;
            Guid teacherId = inputTeacher.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(teacherId))
                    .ReturnsAsync(beforeUpdateStorageTeacher);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTeacherAsync(inputTeacher))
                    .ReturnsAsync(afterUpdateStorageTeacher);

            // when
            Teacher actualTeacher = await this.teacherService.ModifyTeacherAsync(inputTeacher);

            // then
            actualTeacher.Should().BeEquivalentTo(expectedTeacher);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(teacherId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTeacherAsync(inputTeacher),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        
    }
}
