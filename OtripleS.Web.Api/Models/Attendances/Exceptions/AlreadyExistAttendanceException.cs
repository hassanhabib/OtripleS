using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Attendances.Exceptions
{
    public class AlreadyExistAttendanceException:Exception
    {
        public AlreadyExistAttendanceException(Exception innerException)
           : base("Attendance with the same id already exists.", innerException) { }
    }
}
