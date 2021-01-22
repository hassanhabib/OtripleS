// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.TeacherAttachments;
using OtripleS.Web.Api.Models.TeacherAttachments.Exceptions;
using OtripleS.Web.Api.Services.TeacherAttachments;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAttachmentsController : RESTFulController
    {
        private readonly ITeacherAttachmentService teacherAttachmentService;

        public TeacherAttachmentsController(ITeacherAttachmentService teacherAttachmentService) =>
            this.teacherAttachmentService = teacherAttachmentService;

        

        [HttpGet]
        public ActionResult<IQueryable<TeacherAttachment>> GetAllTeacherAttachments()
        {
            try
            {
                IQueryable storageTeacherAttachments =
                    this.teacherAttachmentService.RetrieveAllTeacherAttachments();

                return Ok(storageTeacherAttachments);
            }
            catch (TeacherAttachmentDependencyException teacherAttachmentDependencyException)
            {
                return Problem(teacherAttachmentDependencyException.Message);
            }
            catch (TeacherAttachmentServiceException teacherAttachmentServiceException)
            {
                return Problem(teacherAttachmentServiceException.Message);
            }
        }


        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
