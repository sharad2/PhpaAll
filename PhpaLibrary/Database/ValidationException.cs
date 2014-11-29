using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.PhpaLibrary.Database
{
    public class ValidationException:Exception
    {
        public ValidationException(string message) : base(message)
        {

        }

        public override string ToString()
        {
            return this.Message;
        }
    }
}
