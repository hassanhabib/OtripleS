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
using OtripleS.Web.Api.Models.Teachers;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Teachers
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
        public async Task ShouldRetrieveTeacherByIdAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Guid inputTeacherId = randomTeacher.Id;
            Teacher storageTeacher = randomTeacher;
            Teacher expectedTeacher = randomTeacher;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId))
                    .ReturnsAsync(storageTeacher);

            // when
            Teacher actualTeacher =
                await this.teacherService.RetrieveTeacherByIdAsync(inputTeacherId);

            // then
            actualTeacher.Should().BeEquivalentTo(expectedTeacher);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllTeachers()
        {
            // given
            IQueryable<Teacher> randomTeachers = CreateRandomTeachers();
            IQueryable<Teacher> storageTeachers = randomTeachers;
            IQueryable<Teacher> expectedTeachers = storageTeachers;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeachers())
                    .Returns(storageTeachers);

            // when
            IQueryable<Teacher> actualTeachers =
                this.teacherService.RetrieveAllTeachers();

            // then
            actualTeachers.Should().BeEquivalentTo(expectedTeachers);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeachers(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

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
