using System.Collections.Generic;

namespace SimpleDurableFunctionsSample.Models
{
    public class StartVAWorkflowRequest
    {
        public IEnumerable<string> Participants { get; set; }
    }
}