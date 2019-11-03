using System;

namespace DurableFunctionsV2Sample.Models
{
    public class SendAuditorApprovalRequest
    {
        public Guid VoucherAuditId { get; set; }

        public AuditorApprovalEventData EventData { get; set; }
    }
}