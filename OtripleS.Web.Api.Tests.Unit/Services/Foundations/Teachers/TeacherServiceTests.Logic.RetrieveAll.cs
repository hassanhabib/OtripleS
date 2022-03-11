// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
