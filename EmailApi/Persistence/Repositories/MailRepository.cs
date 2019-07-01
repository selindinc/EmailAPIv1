using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailApi.Contracts;
using EmailApi.Domain.Models;
using EmailApi.Domain.Repositories;
using EmailApi.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EmailApi.Persistence.Repositories
{
    public class MailRepository : BaseRepository, IMailRepository
    {
        public MailRepository(AppDBContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Mail>> ListAsync()
        {
            return await _context.Mails.ToListAsync();
            //tolistasync for transforming the result of the query into
            //a collection of categories

        }
        public Mail GetById(int id)
        {
            return _context.Mails.FirstOrDefault(x => x.MailId == id);
        }

        public Mail CreateMail(Mail mail)
        {
            _context.Add(mail);
            _context.SaveChanges();

            return mail;
            
        }
        public EmailTaskStatuses GetStatus(int mailId)
        {
            var foundMail = GetById(mailId);

            return (EmailTaskStatuses)foundMail.EmailTaskStatus;
        }
        public void UpdateStatus(int mailId, EmailTaskStatuses NewStatus)
        {
            var foundMail = GetById(mailId);
            foundMail.EmailTaskStatus = (int)NewStatus;
            _context.SaveChanges();
        }
        public DateTime GetDate(int mailId)
        {
            var foundMail = GetById(mailId);
            return foundMail.Date;
        }

    }
}
