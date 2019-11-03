namespace SimpleDurableFunctionsSample.Models
{
    public class ProcessAuditorApprovalData
    {
        public int WorkflowId { get; set; }
        public AuditorApprovalEventData ApprovalEventData { get; set; }
    }
}