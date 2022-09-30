using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleOutput
{
    public class Output
    {
        public enum JobStatus
        {
            Success,
            Error
        }

        public string Message { get; set; }
        public string Status { get; set; }
    }
}
