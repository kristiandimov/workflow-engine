using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public class FlowConfigurationHistory
    {
        public int Id { get; set; }
        public string FlowConfig { get; set; }
        public string FlowResult { get; set; }
        public string Status { get; set; }
        public DateTime ExecutionTime { get; set; }
        public int OwnerId { get; set; }

    }
}
