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
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;

            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam nullStudentExam = randomStudentExam;
            var nullStudentExamException = new NullStudentExamException();

            var expectedStudentGuardianValidationException =
                new StudentExamValidationException(nullStudentExamException);

            // when
            ValueTask<StudentExam> addStudentGuardianTask =
                this.studentExamService.AddStudentExamAsync(nullStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                addStudentGuardianTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamAsync(It.IsAny<StudentExam>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}