// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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

        public StudentController(IStudentService studentService) =>
            this.studentService = studentService;

        [HttpPost]
        public async ValueTask<ActionResult<Student>> PostStudentAsync([FromBody] Student student)
        {
            try
            {
                Student registeredStudent =
                        await this.studentService.RegisterStudentAsync(student);

                return Ok(registeredStudent);
            }
            catch (StudentValidationException studentValidationException)
                when (studentValidationException.InnerException is AlreadyExistsStudentException)
            {
                string innerMessage = studentValidationException.InnerException.Message;
                return Conflict(innerMessage);
            }
            catch (StudentValidationException studentValidationException)
            {
                string innerMessage = studentValidationException.InnerException.Message;
                return BadRequest(innerMessage);
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