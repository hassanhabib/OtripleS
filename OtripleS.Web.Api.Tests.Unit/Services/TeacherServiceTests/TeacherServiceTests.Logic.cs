// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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
        public void ShouldRetrieveAllTeachers()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Teacher> randomTeachers = CreateRandomTeachers(randomDateTime);
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
    }
}
