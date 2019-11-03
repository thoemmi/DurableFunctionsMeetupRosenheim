using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctionsV2Sample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DurableFunctionsV2Sample.ExternalFunctions
{
    public class StartVAWorkflow
    {
        [FunctionName("E_HTTP_StartVaWorkflow")]
        public async Task<HttpResponseMessage> StartNewVoucherAuditWorkflow(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient context,
            ILogger log)
        {
            var workflowStartData = JsonConvert.DeserializeObject<StartVAWorkflowRequest>(await req.Content.ReadAsStringAsync());

            string instanceId = await context.StartNewAsync("O_VAWorkflow", workflowStartData);
            log.LogInformation($"Started orchestration with ID = '{instanceId}' for VoucherAuditId = '{workflowStartData.VoucherAuditId}'.");
            return context.CreateCheckStatusResponse(req, instanceId);
        }
    }
}