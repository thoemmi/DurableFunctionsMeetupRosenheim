using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SimpleDurableFunctionsSample.Data.Entities;
using SimpleDurableFunctionsSample.Models;
using SimpleDurableFunctionsSample.Services;

namespace SimpleDurableFunctionsSample.WorkflowProcess.Activities
{
    public class CreateVAActivities
    {
        private readonly IApprovalService _approvalService;

        public CreateVAActivities(IApprovalService approvalService)
        {
            _approvalService = approvalService;
        }

        [FunctionName("A_CreateVoucherAudit")]
        public async Task<int> CreateVoucherAudit(
            [ActivityTrigger] DurableActivityContext context,
            ILogger log)
        {
            var request = context.GetInput<StartVAWorkflowRequest>();

            var workflow = await _approvalService.Create(new WorkflowBaseData()
            {
                InstanceId = context.InstanceId,
                Participants = request.Participants
            });

            return workflow.Id;
        }


        [FunctionName("A_CompleteWorkflow")]
        public async Task CompleteWorkflow(
            [ActivityTrigger] int workflowId, ILogger log)
        {
            await _approvalService.SetStatus(workflowId, WorkflowStatus.Finished);
        }
    }
}