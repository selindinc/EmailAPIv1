using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EmailApi.Contracts;
using EmailApi.Contracts.Events;
using EmailApi.Contracts.Req;
using EmailApi.Contracts.Res;
using EmailApi.CustomExceptions;
using EmailApi.Domain.Models;
using EmailApi.Domain.Repositories;
using EmailApi.Domain.Services;
using MassTransit;
using System.Runtime.InteropServices;

namespace EmailApi.Services
{
    public class MailService : IMailService
    {
        private readonly IMailRepository _mailRepository;

        private readonly IBusControl _busControl;

        public MailService(IMailRepository mailRepository, IBusControl busControl)
        {
            _mailRepository = mailRepository;
            _busControl = busControl;
        }

        public async Task<IEnumerable<Mail>> ListAsync()
        {
            return await _mailRepository.ListAsync();
        }

        public Mail GetMailById(int id)
        {

            return _mailRepository.GetById(id);
        }

        public EmailTaskStatuses GetMailStatus(int mailId)
        {
            return _mailRepository.GetStatus(mailId);
        }
        public bool UpdateStatus(int mailId, EmailTaskStatuses NewStatus)
        {
            
            var mail = GetMailById(mailId);
            if ((EmailTaskStatuses)mail.EmailTaskStatus == EmailTaskStatuses.Cancel && NewStatus == EmailTaskStatuses.Ongoing)
            {
                //throw new CustomExceptions.ValidationException("Mail status cannot be converted to ongoing while the status is cancel");
                return false;
            }
            if ((EmailTaskStatuses)mail.EmailTaskStatus == EmailTaskStatuses.Cancel && NewStatus == EmailTaskStatuses.Pending)
            {
                //throw new CustomExceptions.ValidationException("Mail status cannot be converted to pending while the status is cancel");
                return false;
            }
            if ((EmailTaskStatuses)mail.EmailTaskStatus == EmailTaskStatuses.Cancel && NewStatus == EmailTaskStatuses.Sent)
            {
                //throw new CustomExceptions.ValidationException("Mail status cannot be converted to sent while the status is cancel");
                return false;
            }
            //if ((EmailTaskStatuses)mail.EmailTaskStatus == EmailTaskStatuses.Sent && NewStatus == EmailTaskStatuses.Cancel)
            //{
            //    //throw new CustomExceptions.ValidationException("Mail status cannot be converted to sent while the status is cancel");
            //    return false;
            //}
            if ((EmailTaskStatuses)mail.EmailTaskStatus == EmailTaskStatuses.Sent && NewStatus == EmailTaskStatuses.Pending)
            {
                //throw new CustomExceptions.ValidationException("Mail status cannot be converted to sent while the status is cancel");
                return false;
            }
            if ((EmailTaskStatuses)mail.EmailTaskStatus == EmailTaskStatuses.Sent && NewStatus == EmailTaskStatuses.Ongoing)
            {
                //throw new CustomExceptions.ValidationException("Mail status cannot be converted to sent while the status is cancel");
                return false;
            }
            if ((EmailTaskStatuses)mail.EmailTaskStatus == EmailTaskStatuses.Ongoing && NewStatus == EmailTaskStatuses.Pending)
            {
                //throw new CustomExceptions.ValidationException("Mail status cannot be converted to sent while the status is cancel");
                return false;
            }

            _mailRepository.UpdateStatus(mailId, NewStatus);

            switch (NewStatus)
            {
                case EmailTaskStatuses.Pending:
                    Console.WriteLine("Mail status is pending"); 
                    break;
                case EmailTaskStatuses.Ongoing:
                    Console.WriteLine("Mail status is ongoing"); 
                    break;
                case EmailTaskStatuses.Sent:
                    Console.WriteLine("Mail status is sent");
                    break;
                case EmailTaskStatuses.Cancel:
                    Console.WriteLine("Mail status is cancel");
                    break;
            }
            return true;
            //return Marshal.GetExceptionCode() != 0; //bunun ne döndüğünden bile emin değilim saçma
        }

        public EmailTaskResponse CreateMailTask(CreateEmailSendTask createEmailSendTask)
        {
            if (createEmailSendTask == null)
                throw new CustomExceptions.ValidationException("Argument could not be null");


            if (string.IsNullOrEmpty(createEmailSendTask.ReceiverAddress))
                throw new CustomExceptions.ValidationException("Receiver address should be provided");

          
            if (new EmailAddressAttribute().IsValid(createEmailSendTask.ReceiverAddress)==false)
            {
                throw new CustomExceptions.ValidationException("");
            }

            Mail mail = new Mail
            {
                Content = createEmailSendTask.Content,
                SenderAddress = "my-company@mail.com",
                ReceiverAddress = createEmailSendTask.ReceiverAddress,
                Subject = createEmailSendTask.Subject,
                Date = DateTime.UtcNow,
                EmailTaskStatus = (int)EmailTaskStatuses.Pending,
                ScheduledDateTime = createEmailSendTask.ScheduledDate
        };

            _mailRepository.CreateMail(mail);

            MailScheduledEvent mailScheduledEvent = new MailScheduledEvent
            {
                MailId = mail.MailId,
                ScheduleDate = mail.ScheduledDateTime
            };
            _busControl.Publish(mailScheduledEvent);

            return new EmailTaskResponse()
            {
                EmailId = mail.MailId,
                EmailTaskStatuses = (EmailTaskStatuses)mail.EmailTaskStatus,
                SenderAddress = mail.SenderAddress,
                Subject = mail.Subject,
                ScheduledDateTime = mail.ScheduledDateTime, //bunu ekledim ama eklemeli miydim emin değiilim
                Date=mail.Date ///
            };
        }
    }
}

