using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailApi.CustomExceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message, Exception innerEx = null):base(message, innerEx)
        {
        }
    }
}
