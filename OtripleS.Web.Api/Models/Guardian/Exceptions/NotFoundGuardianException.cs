// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Guardian.Exceptions
{
    public class NotFoundGuardianException:Exception
    {
        public NotFoundGuardianException(Guid guardianId)
        : base($"Couldn't find guardian with Id: {guardianId}.") { }
    }
}
