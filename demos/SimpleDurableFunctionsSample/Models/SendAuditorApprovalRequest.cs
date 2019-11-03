namespace SimpleDurableFunctionsSample.Models
{
    public class SendAuditorApprovalRequest
    {
        public int WorkflowId { get; set; }

        public AuditorApprovalEventData EventData { get; set; }
    }
}