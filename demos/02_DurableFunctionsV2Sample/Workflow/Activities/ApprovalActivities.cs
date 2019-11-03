using System.Linq;
using System.Threading.Tasks;
using DurableFunctionsV2Sample.Models;
using DurableFunctionsV2Sample.Models.Entities;
using ImpromptuInterface.Optimization;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctionsV2Sample.Workflow.Activities
{
    public class ApprovalActivities
    {
        [FunctionName("A_StartApproval")]
        public async Task CreateVoucherAudit(
            [ActivityTrigger] EntityId voucherAuditEntityId,
            [DurableClient] IDurableEntityClient entityClient,
            ILogger log)
        {
            await entityClient.SignalEntityAsync<IVoucherAudit>(voucherAuditEntityId, m => m.SetStatus(VoucherAuditStatus.InApproval));
        }

        [FunctionName("A_ProcessAuditorApproval")]
        public async Task<VoucherAuditStatus> ProcessAuditorApproval(
            [ActivityTrigger] IDurableActivityContext context,
            [DurableClient] IDurableEntityClient entityClient,
            ILogger log)
        {
            var data = context.GetInput<ProcessAuditorApprovalData>();

            if (data.ApprovalEventData.Result)
                await entityClient.SignalEntityAsync<IVoucherAudit>(data.EntityId, m => m.Approve(data.ApprovalEventData.Auditor));
            else
                await entityClient.SignalEntityAsync<IVoucherAudit>(data.EntityId, m => m.Deny(data.ApprovalEventData.Auditor));

            var result = await entityClient.ReadEntityStateAsync<VoucherAudit>(data.EntityId);
            if (result.EntityState.Approvals.Count == result.EntityState.Auditors.Count())
                await entityClient.SignalEntityAsync<IVoucherAudit>(data.EntityId, m => m.SetStatus(VoucherAuditStatus.ApprovalCompleted));

            return result.EntityState.Status;
        }
    }
}