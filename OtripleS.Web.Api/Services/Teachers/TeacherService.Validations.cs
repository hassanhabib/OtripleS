// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;

namespace OtripleS.Web.Api.Services.Teachers
{
    public partial class TeacherService
    {
        private void ValidateTeacherId(Guid teacherId)
        {
            if (teacherId == default)
            {
                throw new InvalidTeacherInputException(
                    parameterName: nameof(Teacher.Id),
                    parameterValue: teacherId);
            }
        }

        private void ValidateStorageTeacher(Teacher maybeTeacher, Guid teacherId)
        {
            if (maybeTeacher is null)
            {
                throw new NotFoundTeacherException(teacherId);
            }
        }

        private void ValidateStorageTeachers(IQueryable<Teacher> storageTeachers)
        {
            if (storageTeachers.Count() == 0)
            {
                this.loggingBroker.LogWarning("No teachers found in storage.");
            }
        }

        private void ValidateTeacherOnCreate(Teacher teacher)
        {
            ValidateTeacher(teacher);
        }

        private void ValidateTeacher(Teacher teacher)
        {
            if (teacher == default)
            {
                throw new NullTeacherException();
            }
        }
    }
}
