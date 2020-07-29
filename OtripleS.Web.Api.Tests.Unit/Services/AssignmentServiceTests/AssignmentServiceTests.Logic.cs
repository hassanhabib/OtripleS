// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Assignments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.AssignmentServiceTests
{
    public partial class AssignmentServiceTests
    {
		[Fact]
		public async Task ShouldCreateAssignmentAsync()
		{
			// given
			DateTimeOffset randomDateTime = GetRandomDateTime();
			DateTimeOffset dateTime = randomDateTime;
			Assignment randomAssignment = CreateRandomAssignment(randomDateTime);
			randomAssignment.UpdatedBy = randomAssignment.CreatedBy;
			randomAssignment.UpdatedDate = randomAssignment.CreatedDate;
			Assignment inputAssignment= randomAssignment;
			Assignment storageAssignment = randomAssignment;
			Assignment expectedAssignment = storageAssignment;

			this.storageBrokerMock.Setup(broker =>
				broker.InsertAssignmentAsync(inputAssignment))
					.ReturnsAsync(storageAssignment);

			// when
			Assignment actualAssignment =
				await this.assignmentService.CreateAssignmentAsync(inputAssignment);

			// then
			actualAssignment.Should().BeEquivalentTo(expectedAssignment);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertAssignmentAsync(inputAssignment),
					Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}
