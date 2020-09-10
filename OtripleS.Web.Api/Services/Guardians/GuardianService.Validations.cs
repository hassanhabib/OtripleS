// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.Guardian;
using OtripleS.Web.Api.Models.Guardian.Exceptions;

namespace OtripleS.Web.Api.Services.Guardians
{
    public partial class GuardianService
    {
        private void ValidateGuardian(Guardian guardian)
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
            }
        }

        private void ValidateGuardianRequiredFields(Guardian guardian)
        {
            switch(guardian)
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
    }
}
