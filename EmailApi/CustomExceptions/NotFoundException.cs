using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailApi.CustomExceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message, Exception innerEx = null):base(message, innerEx)
        {
        }
    }
}
