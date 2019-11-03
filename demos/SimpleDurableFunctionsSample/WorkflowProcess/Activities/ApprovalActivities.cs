using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SimpleDurableFunctionsSample.Data.Entities;
using SimpleDurableFunctionsSample.Models;
using SimpleDurableFunctionsSample.Services;

namespace SimpleDurableFunctionsSample.WorkflowProcess.Activities
{
    public class ApprovalActivities
    {
        private readonly IApprovalService _approvalService;

        public ApprovalActivities(IApprovalService approvalService)
        {
            _approvalService = approvalService;
        }
        
        [FunctionName("A_StartApproval")]
        public async Task CreateVoucherAudit(
            [ActivityTrigger] int workflowId,
            ILogger log)
        {
            await _approvalService.SetStatus(workflowId, WorkflowStatus.InApproval);
        }

        [FunctionName("A_ProcessAuditorApproval")]
        public async Task<WorkflowStatus> ProcessAuditorApproval(
            [ActivityTrigger] DurableActivityContext context,
            ILogger log)
        {
            var data = context.GetInput<ProcessAuditorApprovalData>();

            if (data.ApprovalEventData.Result)
                await _approvalService.Approve(data.WorkflowId, data.ApprovalEventData.Auditor);
            else
                await _approvalService.Deny(data.WorkflowId, data.ApprovalEventData.Auditor);

            if (await _approvalService.IsParticipantInWaitingState(data.WorkflowId))
                return WorkflowStatus.InApproval;
            
            await _approvalService.SetStatus(data.WorkflowId, WorkflowStatus.ApprovalCompleted);
            return WorkflowStatus.ApprovalCompleted;
        }
    }
}