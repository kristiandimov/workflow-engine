using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace WorkflowActionSdk
{
    public interface IWorkflowAction
    {
        public enum WorkflowActionStatus
        {
            Stopped,
            Running
        }

        public WorkflowActionStatus Status { get; set; }

        string Execute(string input, List<ExpandoObject> payload); //changed from string
    }
}
