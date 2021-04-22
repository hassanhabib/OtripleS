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
        private delegate ValueTask<StudentExamFee> ReturningStudentExamFeeFunction();

        private async ValueTask<StudentExamFee> TryCatch(
            ReturningStudentExamFeeFunction returningStudentExamFeeFunction)
        {
            try
            {
                return await returningStudentExamFeeFunction();
            }
            catch (InvalidStudentExamFeeException invalidStudentExamFeeInputException)
            {
                throw CreateAndLogValidationException(invalidStudentExamFeeInputException);
            }
            catch (NotFoundStudentExamFeeException notFoundStudentExamFeeException)
            {
                throw CreateAndLogValidationException(notFoundStudentExamFeeException);
            }
        }

        private StudentExamFeeValidationException CreateAndLogValidationException(Exception exception)
        {
            var StudentExamFeeValidationException = new StudentExamFeeValidationException(exception);
            this.loggingBroker.LogError(StudentExamFeeValidationException);

            return StudentExamFeeValidationException;
        }
    }
}
