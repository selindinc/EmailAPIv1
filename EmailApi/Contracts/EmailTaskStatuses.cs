using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailApi.Contracts
{
    public enum EmailTaskStatuses
    {
        Pending = 2,
        Ongoing = 3,
        Sent = 4,
        Cancel = 5
    }
}
