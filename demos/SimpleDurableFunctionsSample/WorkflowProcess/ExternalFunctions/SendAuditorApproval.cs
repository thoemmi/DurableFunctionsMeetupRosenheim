using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimpleDurableFunctionsSample.Models;
using SimpleDurableFunctionsSample.Services;

namespace SimpleDurableFunctionsSample.WorkflowProcess.ExternalFunctions
{
    public class SendAuditorApproval
    {
        private readonly IApprovalService _approvalService;

        public SendAuditorApproval(IApprovalService approvalService)
        {
            _approvalService = approvalService;
        }

        [FunctionName("E_HTTP_SendAuditorApproval")]
        public async Task SendAuditorApprovalEvent(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequestMessage req,
            [OrchestrationClient] DurableOrchestrationClient context,
            ILogger log)
        {
            var approvalRequest = JsonConvert.DeserializeObject<SendAuditorApprovalRequest>(await req.Content.ReadAsStringAsync());
            var instanceId = await _approvalService.GetInstanceId(approvalRequest.WorkflowId);

            await context.RaiseEventAsync(instanceId,
                $"E_AuditorApprovalEvent_{approvalRequest.WorkflowId.ToString()}",
                approvalRequest.EventData);
        }
    }
}