using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailApi.Contracts.Events
{
    public class MailScheduledEvent
    {
        public int MailId { get; set; }
        public  DateTime ScheduleDate { get; set; }
    }
}
