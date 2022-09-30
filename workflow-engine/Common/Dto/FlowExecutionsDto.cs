using Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Common.Dto
{
    public class FlowExecutionsDto
    {
        public int Id { get; set; }
        public string FlowConfig { get; set; }
        public string FlowResult { get; set; }
        public int OwnerId { get; set; }
        public string Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public UserDto User { get; set; }

    }
}
