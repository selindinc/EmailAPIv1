using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailApi.Contracts.Req
{
    public class CreateEmailSendTask
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public string  ReceiverAddress { get; set; }
        public DateTime ScheduledDate { get; set; }

    }
}
