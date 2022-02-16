﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.ExamFees;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldAddExamFeeAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            ExamFee randomExamFee = CreateRandomExamFee();
            randomExamFee.UpdatedBy = randomExamFee.CreatedBy;
            randomExamFee.UpdatedDate = randomExamFee.CreatedDate;
            ExamFee inputExamFee = randomExamFee;
            ExamFee storageExamFee = randomExamFee;
            ExamFee expectedExamFee = randomExamFee;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamFeeAsync(inputExamFee))
                    .ReturnsAsync(storageExamFee);

            // when
            ExamFee actualExamFee =
                await this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            actualExamFee.Should().BeEquivalentTo(expectedExamFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(inputExamFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllExamFees()
        {
            // given
            IQueryable<ExamFee> randomExamFees =
                CreateRandomExamFees();

            IQueryable<ExamFee> storageExamFees =
                randomExamFees;

            IQueryable<ExamFee> expectedExamFees =
                storageExamFees;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllExamFees())
                    .Returns(storageExamFees);

            // when
            IQueryable<ExamFee> actualExamFees =
                this.examFeeService.RetrieveAllExamFees();

            // then
            actualExamFees.Should().BeEquivalentTo(expectedExamFees);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllExamFees(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveExamFeeByIdAsync()
        {
            // given
            Guid randomExamFeeId = Guid.NewGuid();
            Guid inputExamFeeId = randomExamFeeId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(randomDateTime);
            ExamFee storageExamFee = randomExamFee;
            ExamFee expectedExamFee = storageExamFee;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId))
                    .ReturnsAsync(storageExamFee);

            // when
            ExamFee actualExamFee =
                await this.examFeeService.RetrieveExamFeeByIdAsync(inputExamFeeId);

            // then
            actualExamFee.Should().BeEquivalentTo(expectedExamFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyExamFeeAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(randomInputDate);
            ExamFee inputExamFee = randomExamFee;
            ExamFee afterUpdateStorageExamFee = inputExamFee;
            ExamFee expectedExamFee = afterUpdateStorageExamFee;
            ExamFee beforeUpdateStorageExamFee = randomExamFee.DeepClone();
            inputExamFee.UpdatedDate = randomDate;
            Guid examExamFeeId = inputExamFee.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(examExamFeeId))
                    .ReturnsAsync(beforeUpdateStorageExamFee);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateExamFeeAsync(inputExamFee))
                    .ReturnsAsync(afterUpdateStorageExamFee);

            // when
            ExamFee actualExamFee =
                await this.examFeeService.ModifyExamFeeAsync(inputExamFee);

            // then
            actualExamFee.Should().BeEquivalentTo(expectedExamFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(examExamFeeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateExamFeeAsync(inputExamFee),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldDeleteExamFeeAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            Guid inputExamFeeId = randomExamFee.Id;
            ExamFee inputExamFee = randomExamFee;
            ExamFee storageExamFee = inputExamFee;
            ExamFee expectedExamFee = storageExamFee;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId))
                    .ReturnsAsync(inputExamFee);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteExamFeeAsync(inputExamFee))
                    .ReturnsAsync(storageExamFee);

            // when
            ExamFee actualExamFee =
                await this.examFeeService.RemoveExamFeeByIdAsync(inputExamFeeId);

            //then
            actualExamFee.Should().BeEquivalentTo(expectedExamFee);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamFeeAsync(inputExamFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
