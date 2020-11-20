// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExams
{
    public partial class StudentExamServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentExamIsNullAndLogItAsync()
        {
            // given
            StudentExam randomStudentExam = default;
            StudentExam nullStudentExam = randomStudentExam;
            var nullStudentExamException = new NullStudentExamException();

            var expectedStudentExamValidationException =
                new StudentExamValidationException(nullStudentExamException);

            // when
            ValueTask<StudentExam> addStudentExamTask =
                this.studentExamService.AddStudentExamAsync(nullStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                addStudentExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamAsync(It.IsAny<StudentExam>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}