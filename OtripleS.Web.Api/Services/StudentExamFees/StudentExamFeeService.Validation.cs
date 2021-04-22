using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;

namespace OtripleS.Web.Api.Services.StudentExamFees
{
    public partial class StudentExamFeeService
    {
        private void ValidateStudentExamFeeId(Guid studentExamFeeId)
        {
            if (studentExamFeeId == default)
            {
                throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.Id),
                    parameterValue: studentExamFeeId);
            }
        }
    }
}
