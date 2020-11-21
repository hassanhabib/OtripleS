// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExams
{
    public partial class StudentExamServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenStudentExamIsNullAndLogItAsync()
        {
            //given
            StudentExam invalidStudentExam = null;
            var nullStudentExamException = new NullStudentExamException();

            var expectedStudentExamValidationException =
                new StudentExamValidationException(nullStudentExamException);

            //when
            ValueTask<StudentExam> modifyStudentExamTask =
                this.studentExamService.ModifyStudentExamAsync(invalidStudentExam);

            //then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                modifyStudentExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
