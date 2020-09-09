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

		private void ValidateGuardianOnModify(Guardian guardian)
		{
			ValidateGuardian(guardian);
			ValidateGuardianId(guardian.Id);
			ValidateGuardianIds(guardian);
		}

		private void ValidateGuardian(Guardian guardian)
		{
			if (guardian == null)
			{
				throw new NullGuardianException();
			}
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

		private static bool IsInvalid(Guid input) => input == default;
	}
}
