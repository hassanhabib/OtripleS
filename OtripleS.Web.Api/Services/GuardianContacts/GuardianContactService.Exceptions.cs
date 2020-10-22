//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.GuardianContacts;

namespace OtripleS.Web.Api.Services.GuardianContacts
{
    public partial class GuardianContactService
    {
        private delegate ValueTask<GuardianContact> ReturningGuardianContactFunction();
        

        //private GuardianContactValidationException CreateAndLogValidationException(Exception exception)
        //{
        //    var GuardianContactValidationException = new GuardianContactValidationException(exception);
        //    this.loggingBroker.LogError(GuardianContactValidationException);

        //    return GuardianContactValidationException;
        //}

        //private GuardianContactServiceException CreateAndLogServiceException(Exception exception)
        //{
        //    var GuardianContactServiceException = new GuardianContactServiceException(exception);
        //    this.loggingBroker.LogError(GuardianContactServiceException);

        //    return GuardianContactServiceException;
        //}

        //private GuardianContactDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        //{
        //    var GuardianContactDependencyException = new GuardianContactDependencyException(exception);
        //    this.loggingBroker.LogCritical(GuardianContactDependencyException);

        //    return GuardianContactDependencyException;
        //}

        //private GuardianContactDependencyException CreateAndLogDependencyException(Exception exception)
        //{
        //    var GuardianContactDependencyException = new GuardianContactDependencyException(exception);
        //    this.loggingBroker.LogError(GuardianContactDependencyException);

        //    return GuardianContactDependencyException;
        //}
    }
}