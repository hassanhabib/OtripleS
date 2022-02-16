// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Classrooms;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Classroom> InsertClassroomAsync(Classroom classroom);
        IQueryable<Classroom> SelectAllClassrooms();
        ValueTask<Classroom> SelectClassroomByIdAsync(Guid classroomId);
        ValueTask<Classroom> UpdateClassroomAsync(Classroom classroom);
        ValueTask<Classroom> DeleteClassroomAsync(Classroom classroom);
    }
}
