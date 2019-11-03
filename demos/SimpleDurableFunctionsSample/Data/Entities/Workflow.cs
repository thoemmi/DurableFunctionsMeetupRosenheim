using System;
using System.Collections;
using System.Collections.Generic;

namespace SimpleDurableFunctionsSample.Data.Entities
{
    public class Workflow
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public string InstanceId { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Participant> Participants { get; set; }
    }
}