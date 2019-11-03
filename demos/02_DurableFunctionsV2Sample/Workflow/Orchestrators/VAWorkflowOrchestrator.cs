using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctionsV2Sample.Models;
using DurableFunctionsV2Sample.Models.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunctionsV2Sample.Workflow.Orchestrators
{
    public class VAWorkflowOrchestrator
    {
        [FunctionName("O_VAWorkflow")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger logger)
        {
            var voucherAuditWorkflowRequest = context.GetInput<StartVAWorkflowRequest>();
            EntityId entityId;

            try
            {
                entityId = await context.CallActivityAsync<EntityId>("A_CreateVoucherAudit", voucherAuditWorkflowRequest);
                await context.CallActivityAsync("A_StartApproval", entityId);

                var voucherAuditStatus = VoucherAuditStatus.InApproval;
                while (voucherAuditStatus == VoucherAuditStatus.InApproval)
                {
                    var approvalEventData =
                        await context.WaitForExternalEvent<AuditorApprovalEventData>(
                            $"E_AuditorApprovalEvent_{voucherAuditWorkflowRequest.VoucherAuditId.ToString()}");

                    voucherAuditStatus = await context.CallActivityAsync<VoucherAuditStatus>("A_ProcessAuditorApproval", new ProcessAuditorApprovalData()
                    {
                        EntityId = entityId,
                        ApprovalEventData = approvalEventData
                    });
                }

                // Do some other things ...

                if (voucherAuditStatus == VoucherAuditStatus.ApprovalCompleted)
                    await context.CallActivityAsync<VoucherAudit>("A_CompleteWorkflow", entityId);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in Approval Workflow");
                await context.CallActivityAsync<VoucherAudit>("A_ClearEntity", entityId);
            }
        }
    }
}