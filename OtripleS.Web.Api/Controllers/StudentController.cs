using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Models.Students.Exceptions;
using OtripleS.Web.Api.Services;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpDelete("{studentId}")]
        public async ValueTask<ActionResult<Student>> DeleteStudentAsync(Guid studentId)
        {
            Student storageStudent =
                await this.studentService.DeleteStudentAsync(studentId);

            return Ok(storageStudent);
        }

        [HttpGet("{studentId}")]
        public async ValueTask<ActionResult<Student>> GetStudentAsync(Guid studentId)
        {
            try
            {
                Student storageStudent =
                        await this.studentService.RetrieveStudentByIdAsync(studentId);

                return Ok(storageStudent);
            }
            catch (StudentValidationException studentValidationException)
                when (studentValidationException.InnerException is NotFoundStudentException)
            {
                string innerMessage = studentValidationException.InnerException.Message;
                return NotFound(innerMessage);
            }
            catch (StudentValidationException studentValidationException)
            {
                string innerMessage = studentValidationException.InnerException.Message;
                return BadRequest(studentValidationException);
            }
            catch (StudentDependencyException studentDependencyException)
            {
                return Problem(studentDependencyException.Message);
            }
            catch (StudentServiceException studentServiceException)
            {
                return Problem(studentServiceException.Message);
            }
        }
    }
}