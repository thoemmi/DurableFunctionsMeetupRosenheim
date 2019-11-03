using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimpleDurableFunctionsSample.Models;

namespace SimpleDurableFunctionsSample.WorkflowProcess.ExternalFunctions
{
    public class StartVAWorkflow
    {
        [FunctionName("E_HTTP_StartVaWorkflow")]
        public async Task<HttpResponseMessage> StartNewVoucherAuditWorkflow(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequestMessage req,
            [OrchestrationClient] DurableOrchestrationClient context,
            ILogger log)
        {
            var workflowStartData = JsonConvert.DeserializeObject<StartVAWorkflowRequest>(await req.Content.ReadAsStringAsync());

            string instanceId = await context.StartNewAsync("O_VAWorkflow", workflowStartData);
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            return context.CreateCheckStatusResponse(req, instanceId);
        }
    }
}