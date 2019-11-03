using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace DurableFunctionsV2Sample.Models.Entities
{
    public class VoucherAudit : IVoucherAudit
    {
        public Guid VoucherAuditId { get; set; }

        public IEnumerable<string> Auditors { get; set; }

        public Dictionary<string, bool> Approvals { get; set; } = new Dictionary<string, bool>();

        public VoucherAuditStatus Status { get; set; }

        public string InstanceId { get; set; }


        public void Start(WorkflowBaseData workflowBaseData)
        {
            VoucherAuditId = workflowBaseData.VoucherAuditId;
            Auditors = workflowBaseData.Auditors;
            InstanceId = workflowBaseData.InstanceId;
        }

        public void Approve(string user)
        {
            if (Approvals.ContainsKey(user))
            {
                Approvals[user] = true;
            }
            else
            {
                Approvals.Add(user, true);
            }
        }

        public void Deny(string user)
        {
            if (Approvals.ContainsKey(user))
            {
                Approvals[user] = false;
            }
            else
            {
                Approvals.Add(user, false);
            }
        }

        public void SetStatus(VoucherAuditStatus status)
        {
            Status = status;
        }

        public void Clear()
        {
            Entity.Current.DestructOnExit();
        }

        [FunctionName(nameof(VoucherAudit))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<VoucherAudit>();
    }

    public enum VoucherAuditStatus
    {
        Started,
        InApproval,
        ApprovalCompleted,
        Done
    }
}