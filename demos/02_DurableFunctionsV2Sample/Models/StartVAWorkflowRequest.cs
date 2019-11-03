using System;
using System.Collections.Generic;

namespace DurableFunctionsV2Sample.Models
{
    public class StartVAWorkflowRequest
    {
        public Guid VoucherAuditId { get; set; }

        public IEnumerable<string> Auditors { get; set; }
    }
}