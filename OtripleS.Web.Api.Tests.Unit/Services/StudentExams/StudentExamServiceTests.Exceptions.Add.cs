// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExams
{
    public partial class StudentExamServiceTests
    {

        [Fact]
        public void ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogIt()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam inputStudentExam = randomStudentExam;
            inputStudentExam.UpdatedBy = inputStudentExam.CreatedBy;


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);


            // when
            ValueTask<StudentExam> addStudentExamTask =
                this.studentExamService.AddStudentExamAsync(inputStudentExam);

            // then

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamAsync(inputStudentExam),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam inputStudentExam = randomStudentExam;
            inputStudentExam.UpdatedBy = inputStudentExam.CreatedBy;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentGuardianDependencyException =
                new StudentExamDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamAsync(inputStudentExam))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentExam> addStudentGuardianTask =
                this.studentExamService.AddStudentExamAsync(inputStudentExam);

            // then
            await Assert.ThrowsAsync<StudentExamDependencyException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamAsync(inputStudentExam),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam inputStudentGuardian = randomStudentExam;
            inputStudentGuardian.UpdatedBy = inputStudentGuardian.CreatedBy;
            var exception = new Exception();

            var expectedStudentGuardianServiceException =
                new StudentExamServiceException(exception);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamAsync(inputStudentGuardian))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentExam> addStudentGuardianTask =
                 this.studentExamService.AddStudentExamAsync(inputStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentExamServiceException>(() =>
                addStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamAsync(inputStudentGuardian),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}