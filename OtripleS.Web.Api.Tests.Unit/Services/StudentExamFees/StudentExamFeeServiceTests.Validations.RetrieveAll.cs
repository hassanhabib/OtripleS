//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenStudentExamFeesWereEmptyAndLogIt()
        {
            // given
            IQueryable<StudentExamFee> emptyStorageStudentExamFees =
                new List<StudentExamFee>().AsQueryable();

            IQueryable<StudentExamFee> storageStudentExamFees =
                emptyStorageStudentExamFees;

            IQueryable<StudentExamFee> expectedStudentExamFees =
                emptyStorageStudentExamFees;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentExamFees())
                    .Returns(storageStudentExamFees);

            // when
            IQueryable<StudentExamFee> actualStudentExamFees =
                this.studentExamFeeService.RetrieveAllStudentExamFees();

            // then
            actualStudentExamFees.Should().BeEquivalentTo(expectedStudentExamFees);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentExamFees(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No student exam fees found in storage."),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
