using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailApi.Contracts;
using EmailApi.Domain.Models;

namespace EmailApi.Domain.Repositories
{

    public interface IMailRepository
    {
        Mail GetById(int id);
        void UpdateStatus(int mailId, EmailTaskStatuses NewStatus);
        EmailTaskStatuses GetStatus(int mailId); 
        Mail CreateMail(Mail mail);
        Task<IEnumerable<Mail>> ListAsync();

       
    }
    
}
