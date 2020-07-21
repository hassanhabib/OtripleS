using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Classrooms.Exceptions
{
    public class AlreadyExistsClassroomException : Exception
    {
        public AlreadyExistsClassroomException(Exception innerException)
            : base("Classroom with the same id already exists.", innerException) { }
    }
}
