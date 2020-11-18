// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using Xunit;
using StudentExam = OtripleS.Web.Api.Models.StudentExams.StudentExam;

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
            var nullStudentExamException = new NullStudentGuardianException();

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(nullStudentExamException);

            // when
            ValueTask<StudentExam> addStudentGuardianTask =
                this.studentEaxmService.AddStudentExamAsync(nullStudentExam);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
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