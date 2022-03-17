﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Classrooms;

namespace OtripleS.Web.Api.Services.Foundations.Classrooms
{
    public interface IClassroomService
    {
        ValueTask<Classroom> CreateClassroomAsync(Classroom classroom);
        IQueryable<Classroom> RetrieveAllClassrooms();
        ValueTask<Classroom> RetrieveClassroomById(Guid classroomId);
        ValueTask<Classroom> ModifyClassroomAsync(Classroom classroom);
        ValueTask<Classroom> RemoveClassroomAsync(Guid classroomId);
    }
}
