using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailApi.Contracts;
using EmailApi.Contracts.Req;
using EmailApi.Contracts.Res;
using EmailApi.Domain.Models;
using EmailApi.Domain.Repositories;

namespace EmailApi.Domain.Services
{
    public interface IMailService
    {
        EmailTaskResponse CreateMailTask(CreateEmailSendTask createEmailSendTask);
        Mail GetMailById(int id);
        EmailTaskStatuses GetMailStatus(int mailId);
        Task<IEnumerable<Mail>> ListAsync();
        bool UpdateStatus(int mailId, EmailTaskStatuses NewStatus);




    }
}

