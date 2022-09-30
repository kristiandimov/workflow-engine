using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using WorkflowActionSdk;

namespace ExecutionEngineCli.ExecutionEngine
{
    public class WorkflowActionModel
    {
        public enum WorkflowActionStatus
        {
            Completed,
            Error
        }

        public string Name { get; set; }
        public string ActionIdentifier { get; set; }

        public object Input { get; set; }
        public List<object> Output { get; set; }

        public WorkflowActionStatus Status { get; set; }
    }
}
