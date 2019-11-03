using System;
using System.Collections.Generic;

namespace DurableFunctionsV2Sample.Models
{
    public class WorkflowBaseData
    {
        public string InstanceId { get; set; }
        
        public Guid VoucherAuditId { get; set; }

        public IEnumerable<string> Auditors { get; set; }
    }
}