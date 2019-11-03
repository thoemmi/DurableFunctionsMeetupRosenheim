namespace DurableFunctionsV2Sample.Models.Entities
{
    public interface IVoucherAudit
    {
        void Start(WorkflowBaseData workflowBaseData);
        void Approve(string user);
        void Deny(string user);
        void SetStatus(VoucherAuditStatus status);
        void Clear();
    }
}