namespace SimpleDurableFunctionsSample.Data.Entities
{
    public enum WorkflowStatus
    {
        Created,
        Started,
        InApproval,
        ApprovalCompleted,
        Finished
    }
}