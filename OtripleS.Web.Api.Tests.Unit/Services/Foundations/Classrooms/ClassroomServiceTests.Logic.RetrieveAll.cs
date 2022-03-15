// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Classrooms;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Classrooms
{
    public partial class ClassroomServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllClassrooms()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Classroom> randomClassrooms = CreateRandomClassrooms(randomDateTime);
            IQueryable<Classroom> storageClassrooms = randomClassrooms;
            IQueryable<Classroom> expectedClassrooms = storageClassrooms;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClassrooms())
                    .Returns(storageClassrooms);

            // when
            IQueryable<Classroom> actualClassrooms =
                this.classroomService.RetrieveAllClassrooms();

            // then
            actualClassrooms.Should().BeEquivalentTo(expectedClassrooms);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllClassrooms(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
