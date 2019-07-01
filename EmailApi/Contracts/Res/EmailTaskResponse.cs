using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailApi.Contracts.Res
{
    public class EmailTaskResponse
    {
        public int EmailId { get; set; }
        public string Subject { get; set; }
        public string SenderAddress { get; set; }
        public EmailTaskStatuses EmailTaskStatuses { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public DateTime Date { get; set; }
    }
}
