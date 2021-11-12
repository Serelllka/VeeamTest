using System;

namespace Veeam.Tools
{
    public class VeeamException : Exception
    {
        public VeeamException()
        {
        }

        public VeeamException(string message)
            : base(message)
        {
        }

        public VeeamException(string message, Exception innerException)
            : base(message, innerException)

        {
        }
    }
}