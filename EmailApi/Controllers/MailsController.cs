using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmailApi.Domain.Models;
using EmailApi.Services;
using EmailApi.Contracts.Req;
using System.Net;
using EmailApi.Domain.Services;
using EmailApi.Domain.Repositories;
using EmailApi.Persistence.Contexts;
using EmailApi.Contracts;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmailApi.Controllers
{
    [Route("mails")]
    public class MailsController : Controller
    {
        private readonly IMailService _mailService;
        private readonly CreateEmailSendTask _createEmailSendTask;

        public MailsController(IMailService mailService)
        {
            _mailService = mailService;
        }


        [HttpPost("send-email-task")]
        public IActionResult CreateSendMailTask([FromBody] CreateEmailSendTask createEmailSendTask)
        {
            var createdMail = _mailService.CreateMailTask(createEmailSendTask);

            return StatusCode((int)HttpStatusCode.Created, createdMail);
        }

        //GET mail
        [HttpGet("")]
        public async Task<IEnumerable<Mail>> ListAsync()
        {
            var mails = await _mailService.ListAsync();

            return mails;
        }

        //GET mail/id
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            var mail = _mailService.GetMailById(id);
            if (mail == null)
            {
                return NotFound();
            }

            return Ok(mail);
        }

        [HttpGet("{mailId}/status")]
        public IActionResult Put([FromRoute] int mailId)
        {
            var mail = _mailService.GetMailById(mailId);

            return Ok(mail.EmailTaskStatus);
        }

        //PUT mail/id/status
        [HttpPut("{mailId}/status")]
        public IActionResult Put([FromRoute] int mailId,[FromBody] EmailTaskStatuses NewStatus)
        {

            _mailService.UpdateStatus(mailId, NewStatus);

            return NoContent();
            
        }
    }
}
