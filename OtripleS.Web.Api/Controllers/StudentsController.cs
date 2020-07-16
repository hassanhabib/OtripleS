// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Models.Students.Exceptions;
using OtripleS.Web.Api.Services.Students;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : RESTFulController
    {
        private readonly IStudentService studentService;

        public StudentsController(IStudentService studentService) =>
            this.studentService = studentService;

        [HttpPost]
        public async ValueTask<ActionResult<Student>> PostStudentAsync(Student student)
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
                string innerMessage = GetInnerMessage(studentValidationException);

                return Conflict(innerMessage);
            }
            catch (StudentValidationException studentValidationException)
            {
                string innerMessage = GetInnerMessage(studentValidationException);

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

        [HttpGet]
        public ActionResult<IQueryable<Student>> GetAllStudents()
        {
            try
            {
                IQueryable<Student> storageStudents =
                    this.studentService.RetrieveAllStudents();

                return Ok(storageStudents);
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
                string innerMessage = GetInnerMessage(studentValidationException);

                return NotFound(innerMessage);
            }
            catch (StudentValidationException studentValidationException)
            {
                string innerMessage = GetInnerMessage(studentValidationException);

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

        [HttpPut]
        public async ValueTask<ActionResult<Student>> PutStudentAsync(Student student)
        {
            try
            {
                Student registeredStudent =
                    await this.studentService.ModifyStudentAsync(student);

                return Ok(registeredStudent);
            }
            catch (StudentValidationException studentValidationException)
                when (studentValidationException.InnerException is NotFoundStudentException)
            {
                string innerMessage = GetInnerMessage(studentValidationException);

                return NotFound(innerMessage);
            }
            catch (StudentValidationException studentValidationException)
            {
                string innerMessage = GetInnerMessage(studentValidationException);

                return BadRequest(innerMessage);
            }
            catch (StudentDependencyException studentDependencyException)
                when (studentDependencyException.InnerException is LockedStudentException)
            {
                string innerMessage = GetInnerMessage(studentDependencyException);

                return Locked(innerMessage);
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
            try
            {
                Student storageStudent =
                    await this.studentService.DeleteStudentAsync(studentId);

                return Ok(storageStudent);
            }
            catch (StudentValidationException studentValidationException)
                when (studentValidationException.InnerException is NotFoundStudentException)
            {
                string innerMessage = GetInnerMessage(studentValidationException);

                return NotFound(innerMessage);
            }
            catch (StudentValidationException studentValidationException)
            {
                string innerMessage = GetInnerMessage(studentValidationException);

                return BadRequest(studentValidationException);
            }
            catch (StudentDependencyException studentDependencyException)
               when (studentDependencyException.InnerException is LockedStudentException)
            {
                string innerMessage = GetInnerMessage(studentDependencyException);

                return Locked(innerMessage);
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

        public static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}