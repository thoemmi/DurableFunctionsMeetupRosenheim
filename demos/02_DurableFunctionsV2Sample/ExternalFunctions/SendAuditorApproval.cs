using System;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunctionsV2Sample.Models;
using DurableFunctionsV2Sample.Models.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DurableFunctionsV2Sample.ExternalFunctions
{
    public class SendAuditorApproval
    {
        [FunctionName("E_HTTP_SendAuditorApproval")]
        public async Task SendAuditorApprovalEvent(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient context,
            [DurableClient] IDurableEntityClient entityClient,
            ILogger log)
        {
            var approvalRequest = JsonConvert.DeserializeObject<SendAuditorApprovalRequest>(await req.Content.ReadAsStringAsync());

            var key = new EntityId(nameof(VoucherAudit), approvalRequest.VoucherAuditId.ToString());
            var result = await entityClient.ReadEntityStateAsync<VoucherAudit>(key);
            if (result.EntityExists)
            {
                await context.RaiseEventAsync(result.EntityState.InstanceId,
                    $"E_AuditorApprovalEvent_{approvalRequest.VoucherAuditId.ToString()}",
                    approvalRequest.EventData);
            }
            else
            {
                throw new NullReferenceException($"Can't find a VA with id '{approvalRequest.VoucherAuditId}'");
            }
            
            
        }
    }
}