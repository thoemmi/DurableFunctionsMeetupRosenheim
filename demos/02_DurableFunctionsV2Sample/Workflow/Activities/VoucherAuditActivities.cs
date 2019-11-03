using System;
using System.Threading.Tasks;
using DurableFunctionsV2Sample.Models;
using DurableFunctionsV2Sample.Models.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DurableFunctionsV2Sample.Workflow.Activities
{
    public class CreateVAActivities
    {
        [FunctionName("A_CreateVoucherAudit")]
        public async Task<EntityId> CreateVoucherAudit(
            [ActivityTrigger] IDurableActivityContext context,
            [DurableClient] IDurableEntityClient entityClient,
            ILogger log)
        {
            var request = context.GetInput<StartVAWorkflowRequest>();

            var voucherAuditEntityId = new EntityId(nameof(VoucherAudit), request.VoucherAuditId.ToString());
            var result = await entityClient.ReadEntityStateAsync<VoucherAudit>(voucherAuditEntityId);
            if (result.EntityExists)
            {
                throw new ApplicationException($"Workflow for VoucherAudit '{request.VoucherAuditId}' already started.");
            }

            var workflowBaseData = new WorkflowBaseData()
            {
                InstanceId = context.InstanceId,
                Auditors = request.Auditors,
                VoucherAuditId = request.VoucherAuditId
            };

            await entityClient.SignalEntityAsync<IVoucherAudit>(voucherAuditEntityId, m => m.Start(workflowBaseData));
            await entityClient.SignalEntityAsync<IVoucherAudit>(voucherAuditEntityId, m => m.SetStatus(VoucherAuditStatus.Started));

            return voucherAuditEntityId;
        }


        [FunctionName("A_CompleteWorkflow")]
        public async Task<VoucherAudit> CompleteWorkflow(
            [ActivityTrigger] EntityId voucherAuditEntityId,
            [DurableClient] IDurableEntityClient entityClient,
            ILogger log)
        {
            var result = await entityClient.ReadEntityStateAsync<VoucherAudit>(voucherAuditEntityId);
            if (!result.EntityExists)
            {
                throw new NullReferenceException("Entity doesn't exists.");
            }

            await entityClient.SignalEntityAsync<IVoucherAudit>(voucherAuditEntityId, m => m.SetStatus(VoucherAuditStatus.Done));
            return result.EntityState;
        }
        
        [FunctionName("A_ClearEntity")]
        public async Task ClearEntity(
            [ActivityTrigger] EntityId voucherAuditEntityId,
            [DurableClient] IDurableEntityClient entityClient,
            ILogger log)
        {
            await entityClient.SignalEntityAsync<IVoucherAudit>(voucherAuditEntityId, m => m.Clear());
        }
    }
}