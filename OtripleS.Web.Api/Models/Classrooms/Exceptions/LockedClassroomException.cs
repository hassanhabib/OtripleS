using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Classrooms.Exceptions
{
    public class LockedClassroomException:Exception
    {
        public LockedClassroomException(Exception innerException)
           : base("Locked Classroom record exception, please try again later.", innerException) { }
    }
}
