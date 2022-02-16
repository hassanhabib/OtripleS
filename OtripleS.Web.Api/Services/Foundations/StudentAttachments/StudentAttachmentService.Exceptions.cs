﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentAttachments;
using OtripleS.Web.Api.Models.StudentAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentService
    {
        private delegate ValueTask<StudentAttachment> ReturningStudentAttachmentFunction();
        private delegate IQueryable<StudentAttachment> ReturningStudentAttachmentsFunction();

        private async ValueTask<StudentAttachment> TryCatch(
            ReturningStudentAttachmentFunction returningStudentAttachmentFunction)
        {
            try
            {
                return await returningStudentAttachmentFunction();
            }
            catch (NullStudentAttachmentException nullStudentAttachmentInputException)
            {
                throw CreateAndLogValidationException(nullStudentAttachmentInputException);
            }
            catch (InvalidStudentAttachmentException invalidStudentAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidStudentAttachmentInputException);
            }
            catch (NotFoundStudentAttachmentException notFoundStudentAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundStudentAttachmentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsStudentAttachmentException =
                    new AlreadyExistsStudentAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsStudentAttachmentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidStudentAttachmentReferenceException =
                    new InvalidStudentAttachmentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogValidationException(invalidStudentAttachmentReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAttachmentException =
                    new LockedStudentAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedAttachmentException);
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

        private IQueryable<StudentAttachment> TryCatch(ReturningStudentAttachmentsFunction returningStudentAttachmentsFunction)
        {
            try
            {
                return returningStudentAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private StudentAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var studentAttachmentValidationException = new StudentAttachmentValidationException(exception);
            this.loggingBroker.LogError(studentAttachmentValidationException);

            return studentAttachmentValidationException;
        }

        private StudentAttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var studentAttachmentDependencyException = new StudentAttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(studentAttachmentDependencyException);

            return studentAttachmentDependencyException;
        }

        private StudentAttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var studentAttachmentDependencyException = new StudentAttachmentDependencyException(exception);
            this.loggingBroker.LogError(studentAttachmentDependencyException);

            return studentAttachmentDependencyException;
        }

        private StudentAttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var studentAttachmentServiceException = new StudentAttachmentServiceException(exception);
            this.loggingBroker.LogError(studentAttachmentServiceException);

            return studentAttachmentServiceException;
        }
    }
}
