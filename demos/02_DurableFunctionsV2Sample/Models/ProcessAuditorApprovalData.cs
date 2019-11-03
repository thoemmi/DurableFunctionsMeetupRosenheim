using System;
using Microsoft.Azure.WebJobs;

namespace DurableFunctionsV2Sample.Models
{
    public class ProcessAuditorApprovalData
    {
        public AuditorApprovalEventData ApprovalEventData { get; set; }

        public EntityId EntityId { get; set; }
    }
}