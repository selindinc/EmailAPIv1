using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailApi.Domain.Models
{
    public class Mail
    {
        public int MailId { get; set; }
        public string SenderAddress { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string ReceiverAddress { get; set; }
        public int EmailTaskStatus { get; set; }
        public DateTime ScheduledDateTime { get; set; }
    }
}
