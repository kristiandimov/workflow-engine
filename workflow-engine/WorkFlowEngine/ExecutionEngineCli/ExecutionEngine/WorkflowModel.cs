using System;
using System.Collections.Generic;
using System.Text;

namespace ExecutionEngineCli.ExecutionEngine
{
    public class WorkflowModel
    {
        public string Name { get; set; }

        public List<WorkflowActionModel> Actions { get; set; }
    }
}
