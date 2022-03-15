// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldAddStudentExamFeeAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee();
            randomStudentExamFee.UpdatedBy = randomStudentExamFee.CreatedBy;
            randomStudentExamFee.UpdatedDate = randomStudentExamFee.CreatedDate;
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            StudentExamFee storageStudentExamFee = randomStudentExamFee;
            StudentExamFee expectedStudentExamFee = randomStudentExamFee;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamFeeAsync(inputStudentExamFee))
                    .ReturnsAsync(storageStudentExamFee);

            // when
            StudentExamFee actualStudentExamFee =
                await this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            actualStudentExamFee.Should().BeEquivalentTo(expectedStudentExamFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamFeeAsync(inputStudentExamFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
