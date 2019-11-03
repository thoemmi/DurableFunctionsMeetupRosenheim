using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SimpleDurableFunctionsSample.Data.Entities;
using SimpleDurableFunctionsSample.Models;

namespace SimpleDurableFunctionsSample.WorkflowProcess.Orchestrators
{
    public class VAWorkflowOrchestrator
    {
        [FunctionName("O_VAWorkflow")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context,
            ILogger logger)
        {
            var voucherAuditWorkflowRequest = context.GetInput<StartVAWorkflowRequest>();
            int workflowId = 0;

            try
            {
                workflowId = await context.CallActivityAsync<int>("A_CreateVoucherAudit", voucherAuditWorkflowRequest);
                await context.CallActivityAsync("A_StartApproval", workflowId);

                var voucherAuditStatus = WorkflowStatus.InApproval;
                while (voucherAuditStatus == WorkflowStatus.InApproval)
                {
                    var approvalEventData =
                        await context.WaitForExternalEvent<AuditorApprovalEventData>(
                            $"E_AuditorApprovalEvent_{workflowId.ToString()}");

                    voucherAuditStatus = await context.CallActivityAsync<WorkflowStatus>("A_ProcessAuditorApproval", new ProcessAuditorApprovalData()
                    {
                        WorkflowId = workflowId,
                        ApprovalEventData = approvalEventData
                    });
                }

                // Do some other things ...

                if (voucherAuditStatus == WorkflowStatus.ApprovalCompleted)
                    await context.CallActivityAsync("A_CompleteWorkflow", workflowId);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in Approval Workflow");
            }
        }
    }
}