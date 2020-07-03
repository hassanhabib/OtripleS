using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Students.Exceptions
{
    public class NullStudentException : Exception
    {
        public NullStudentException()
            : base("The student is null.") { }
    }
}
