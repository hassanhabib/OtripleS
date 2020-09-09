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
            if (guardianId == default)
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
