// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;
using OtripleS.Web.Api.Services.Exams;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : RESTFulController
    {
        private readonly IExamService examService;

        public ExamsController(IExamService examService) =>
            this.examService = examService;

        [HttpPost]
        public async ValueTask<ActionResult<Exam>> PostExamAsync(Exam exam)
        {
            try
            {
                Exam persistedExam =
                    await this.examService.AddExamAsync(exam);

                return Ok(persistedExam);
            }
            catch (ExamValidationException examValidationException)
                when (examValidationException.InnerException is AlreadyExistsExamException)
            {
                string innerMessage = GetInnerMessage(examValidationException);

                return Conflict(innerMessage);
            }
            catch (ExamValidationException examValidationException)
            {
                string innerMessage = GetInnerMessage(examValidationException);

                return BadRequest(innerMessage);
            }
            catch (ExamDependencyException examDependencyException)
            {
                return Problem(examDependencyException.Message);
            }
            catch (ExamServiceException examServiceException)
            {
                return Problem(examServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Exam>> GetAllExams()
        {
            try
            {
                IQueryable storageExam =
                    this.examService.RetrieveAllExams();

                return Ok(storageExam);
            }
            catch (ExamDependencyException examDependencyException)
            {
                return Problem(examDependencyException.Message);
            }
            catch (ExamServiceException examServiceException)
            {
                return Problem(examServiceException.Message);
            }
        }

        [HttpGet("{examId}")]
        public async ValueTask<ActionResult<Exam>> GetExamAsync(Guid examId)
        {
            try
            {
                Exam storageExam =
                    await this.examService.RetrieveExamByIdAsync(examId);

                return Ok(storageExam);
            }
            catch (ExamValidationException examValidationException)
                when (examValidationException.InnerException is NotFoundExamException)
            {
                string innerMessage = GetInnerMessage(examValidationException);

                return NotFound(innerMessage);
            }
            catch (ExamValidationException examValidationException)
            {
                string innerMessage = GetInnerMessage(examValidationException);

                return BadRequest(innerMessage);
            }
            catch (ExamDependencyException examDependencyException)
            {
                return Problem(examDependencyException.Message);
            }
            catch (ExamServiceException examServiceException)
            {
                return Problem(examServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Exam>> PutExamAsync(Exam exam)
        {
            try
            {
                Exam registeredExam =
                    await this.examService.ModifyExamAsync(exam);

                return Ok(registeredExam);
            }
            catch (ExamValidationException examValidationException)
                when (examValidationException.InnerException is NotFoundExamException)
            {
                string innerMessage = GetInnerMessage(examValidationException);

                return NotFound(innerMessage);
            }
            catch (ExamValidationException examValidationException)
            {
                string innerMessage = GetInnerMessage(examValidationException);

                return BadRequest(innerMessage);
            }
            catch (ExamDependencyException examDependencyException)
                when (examDependencyException.InnerException is LockedExamException)
            {
                string innerMessage = GetInnerMessage(examDependencyException);

                return Locked(innerMessage);
            }
            catch (ExamDependencyException examDependencyException)
            {
                return Problem(examDependencyException.Message);
            }
            catch (ExamServiceException examServiceException)
            {
                return Problem(examServiceException.Message);
            }
        }

        [HttpDelete("{examId}")]
        public async ValueTask<ActionResult<Exam>> DeleteExamAsync(Guid examId)
        {
            try
            {
                Exam storageExam =
                    await this.examService.DeleteExamByIdAsync(examId);

                return Ok(storageExam);
            }
            catch (ExamValidationException examValidationException)
                when (examValidationException.InnerException is NotFoundExamException)
            {
                string innerMessage = GetInnerMessage(examValidationException);

                return NotFound(innerMessage);
            }
            catch (ExamValidationException examValidationException)
            {
                string innerMessage = GetInnerMessage(examValidationException);

                return BadRequest(examValidationException);
            }
            catch (ExamDependencyException examDependencyException)
               when (examDependencyException.InnerException is LockedExamException)
            {
                string innerMessage = GetInnerMessage(examDependencyException);

                return Locked(innerMessage);
            }
            catch (ExamDependencyException examDependencyException)
            {
                return Problem(examDependencyException.Message);
            }
            catch (ExamServiceException examServiceException)
            {
                return Problem(examServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
