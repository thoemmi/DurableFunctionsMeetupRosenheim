using System.Collections.Generic;

namespace SimpleDurableFunctionsSample.Models
{
    public class WorkflowBaseData
    {
        public string InstanceId { get; set; }

        public IEnumerable<string> Participants { get; set; }
    }
}