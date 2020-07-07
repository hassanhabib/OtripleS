using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class LockedTeacherException : Exception
    {
        public LockedTeacherException(Exception innerException)
            : base("Locked teacher record exception, please try again later.", innerException) { }
    }
}
