using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Common.Entities
{
    public class FlowExecutions
    {
        public int Id { get; set; }
        public string FlowConfig { get; set; }
        public string FlowResult { get; set; }
        public int OwnerId { get; set; }
        public string Status { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime UpdateTime { get; set; } = DateTime.Now;

        [JsonIgnore]
        [ForeignKey(nameof(OwnerId))]
        public virtual User User { get; set; }
    }
}
