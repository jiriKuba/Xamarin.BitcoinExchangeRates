using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.Curses.Messages
{
    public class ExceptionMessage
    {
        public Exception ThrowedException { get; private set; }

        public ExceptionMessage(Exception throwedException)
        {
            ThrowedException = throwedException;
        }
    }
}
