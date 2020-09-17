// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Guardians;
using OtripleS.Web.Api.Models.Guardians.Exceptions;

namespace OtripleS.Web.Api.Services.Guardians
{
    public partial class GuardianService
    {
        private void ValidateGuardianOnCreate(Guardian guardian)
        {
            ValidateGuardianIdIsNotNull(guardian);
            ValidateGuardianId(guardian.Id);
            ValidateGuardianRequiredFields(guardian);
            ValidateGuardianAuditFieldsOnCreate(guardian);
        }

        private void ValidateGuardianAuditFieldsOnCreate(Guardian guardian)
        {
            switch (guardian)
            {
                case { } when IsInvalid(input: guardian.CreatedBy):
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.CreatedBy),
                        parameterValue: guardian.CreatedBy);

                case { } when IsInvalid(input: guardian.CreatedDate):
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.CreatedDate),
                        parameterValue: guardian.CreatedDate);

                case { } when IsInvalid(input: guardian.UpdatedBy):
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.UpdatedBy),
                        parameterValue: guardian.UpdatedBy);

                case { } when IsInvalid(input: guardian.UpdatedDate):
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.UpdatedDate),
                        parameterValue: guardian.UpdatedDate);

                case { } when guardian.UpdatedBy != guardian.CreatedBy:
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.UpdatedBy),
                        parameterValue: guardian.UpdatedBy);

                case { } when guardian.UpdatedDate != guardian.CreatedDate:
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.UpdatedDate),
                        parameterValue: guardian.UpdatedDate);

                case { } when IsDateNotRecent(guardian.CreatedDate):
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.CreatedDate),
                        parameterValue: guardian.CreatedDate);
            }
        }

        private void ValidateGuardianRequiredFields(Guardian guardian)
        {
            switch (guardian)
            {
                case { } when IsInvalid(input: guardian.FirstName):
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.FirstName),
                        parameterValue: guardian.FirstName);

                case { } when IsInvalid(input: guardian.FamilyName):
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.FamilyName),
                        parameterValue: guardian.FamilyName);
            }
        }

        private bool IsInvalid(string input) => string.IsNullOrWhiteSpace(input);
        private bool IsInvalid(Guid input) => input == default;
        private bool IsInvalid(DateTimeOffset input) => input == default;

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private void ValidateGuardianIdIsNotNull(Guardian guardian)
        {
            if (guardian == default)
            {
                throw new NullGuardianException();
            }
        }

        private void ValidateGuardianId(Guid guardianId)
        {
            if (IsInvalid(guardianId))
            {
                throw new InvalidGuardianException(
                    parameterName: nameof(Guardian.Id),
                    parameterValue: guardianId);
            }
        }

        private void ValidateStorageGuardian(Guardian storageGuardian, Guid guardianId)
        {
            if (storageGuardian == null)
            {
                throw new NotFoundGuardianException(guardianId);
            }
        }

        private void ValidateGuardianOnModify(Guardian guardian)
        {
            ValidateGuardianIdIsNotNull(guardian);
            ValidateGuardianId(guardian.Id);
            ValidateGuardianIds(guardian);
            ValidateGuardianDates(guardian);
            ValidateDatesAreNotSame(guardian);
            ValidateUpdatedDateIsRecent(guardian);
        }

        private void ValidateGuardianIds(Guardian guardian)
        {
            switch (guardian)
            {
                case { } when IsInvalid(guardian.CreatedBy):
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.CreatedBy),
                        parameterValue: guardian.CreatedBy);

                case { } when IsInvalid(guardian.UpdatedBy):
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.UpdatedBy),
                        parameterValue: guardian.UpdatedBy);
            }
        }

        private void ValidateGuardianDates(Guardian guardian)
        {
            switch (guardian)
            {
                case { } when guardian.CreatedDate == default:
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.CreatedDate),
                        parameterValue: guardian.CreatedDate);

                case { } when guardian.UpdatedDate == default:
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.UpdatedDate),
                        parameterValue: guardian.UpdatedDate);
            }
        }

        private void ValidateDatesAreNotSame(Guardian guardian)
        {
            if (guardian.CreatedDate == guardian.UpdatedDate)
            {
                throw new InvalidGuardianException(
                    parameterName: nameof(guardian.UpdatedDate),
                    parameterValue: guardian.UpdatedDate);
            }
        }

        private void ValidateUpdatedDateIsRecent(Guardian guardian)
        {
            if (IsDateNotRecent(guardian.UpdatedDate))
            {
                throw new InvalidGuardianException(
                    parameterName: nameof(guardian.UpdatedDate),
                    parameterValue: guardian.UpdatedDate);
            }
        }

        private void ValidateAgainstStorageGuardianOnModify(Guardian inputGuardian, Guardian storageGuardian)
        {
            switch (inputGuardian)
            {
                case { } when inputGuardian.CreatedDate != storageGuardian.CreatedDate:
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.CreatedDate),
                        parameterValue: inputGuardian.CreatedDate);

                case { } when inputGuardian.CreatedBy != storageGuardian.CreatedBy:
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.CreatedBy),
                        parameterValue: inputGuardian.CreatedBy);

                case { } when inputGuardian.UpdatedDate == storageGuardian.UpdatedDate:
                    throw new InvalidGuardianException(
                        parameterName: nameof(Guardian.UpdatedDate),
                        parameterValue: inputGuardian.UpdatedDate);
            }
        }

        private void ValidateStorageGuardians(IQueryable<Guardian> storageGuardians)
        {
            if (storageGuardians.Count() == 0)
            {
                this.loggingBroker.LogWarning("No guardians found in storage.");
            }
        }
    }
}
