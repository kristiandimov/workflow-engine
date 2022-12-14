using System;
using System.Collections.Generic;
using System.Text;

namespace S3Writer
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
        public bool GeneratedFile { get; set; } = false;

    }
}

