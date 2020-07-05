// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Models.Students.Exceptions;
using OtripleS.Web.Api.Requests;
using OtripleS.Web.Api.Services;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService) =>
            this.studentService = studentService;

        [HttpDelete("{studentId}")]
        public async ValueTask<ActionResult<Student>> DeleteStudentAsync(Guid studentId)
        {
            Student storageStudent =
                await this.studentService.DeleteStudentAsync(studentId);

            return Ok(storageStudent);
        }

        [HttpPut("{studentId}")]
        public async ValueTask<ActionResult<Student>> UpdateStudentAsync(Guid studentId,
            [FromBody] StudentUpdateDto dto)
        {
            try
            {
                var storageStudent = await this.studentService.ModifyStudentAsync(studentId, dto);
                return Ok(storageStudent);
            }
            catch (StudentValidationException studentValidationException)
                when (studentValidationException.InnerException is NotFoundStudentException)
            {
                return NotFound(studentValidationException.InnerException.Message);
            }
            catch (StudentValidationException studentValidationException)
            {
                return BadRequest(studentValidationException.Message);
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