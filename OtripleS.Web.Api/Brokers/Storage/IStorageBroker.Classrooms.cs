// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Classrooms;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        ValueTask<Classroom> InsertClassroomAsync(Classroom Classroom);
        IQueryable<Classroom> SelectAllClassrooms();
        ValueTask<Classroom> SelectClassroomByIdAsync(Guid ClassroomId);
        ValueTask<Classroom> UpdateClassroomAsync(Classroom Classroom);
        ValueTask<Classroom> DeleteClassroomAsync(Classroom Classroom);
    }
}
