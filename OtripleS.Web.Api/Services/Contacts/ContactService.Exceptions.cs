// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.Contacts.Exceptions;

namespace OtripleS.Web.Api.Services.Contacts
{
    public partial class ContactService
    {
        private delegate ValueTask<Contact> ReturningContactFunction();
        private delegate IQueryable<Contact> ReturningQueryableContactFunction();

        private async ValueTask<Contact> TryCatch(ReturningContactFunction returningContactFunction)
        {
            try
            {
                return await returningContactFunction();
            }
            catch (NullContactException nullContactException)
            {
                throw CreateAndLogValidationException(nullContactException);
            }
            catch (NotFoundContactException notFoundContactException)
            {
                throw CreateAndLogValidationException(notFoundContactException);
            }
            catch (InvalidContactException invalidContactException)
            {
                throw CreateAndLogValidationException(invalidContactException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsContactException =
                new AlreadyExistsContactException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsContactException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedContactException =
                    new LockedContactException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedContactException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private IQueryable<Contact> TryCatch(ReturningQueryableContactFunction returningQueryableContactFunction)
        {
            try
            {
                return returningQueryableContactFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private ContactValidationException CreateAndLogValidationException(Exception exception)
        {
            var contactValidationException = new ContactValidationException(exception);
            this.loggingBroker.LogError(contactValidationException);

            return contactValidationException;
        }

        private ContactDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var contactDependencyException = new ContactDependencyException(exception);
            this.loggingBroker.LogCritical(contactDependencyException);

            return contactDependencyException;
        }

        private ContactDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var contactDependencyException = new ContactDependencyException(exception);
            this.loggingBroker.LogError(contactDependencyException);

            return contactDependencyException;
        }

        private ContactServiceException CreateAndLogServiceException(Exception exception)
        {
            var contactServiceException = new ContactServiceException(exception);
            this.loggingBroker.LogError(contactServiceException);

            return contactServiceException;
        }
    }
}
