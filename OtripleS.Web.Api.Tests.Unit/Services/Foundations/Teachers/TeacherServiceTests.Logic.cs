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
using OtripleS.Web.Api.Models.Teachers;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Teachers
{
    public partial class TeacherServiceTests
    {        
        
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
    }
}
