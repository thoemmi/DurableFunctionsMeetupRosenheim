using System.Threading.Tasks;
using SimpleDurableFunctionsSample.Data.Entities;
using SimpleDurableFunctionsSample.Models;

namespace SimpleDurableFunctionsSample.Services
{
    public interface IApprovalService
    {
        Task<Workflow> Create(WorkflowBaseData workflowBaseData);
        Task SetStatus(int workflowId, WorkflowStatus status);
        Task Approve(int workflowId, string userName);
        Task Deny(int workflowId, string userName);
        Task<bool> IsParticipantInWaitingState(int workflowId);
        Task<string> GetInstanceId(int workflowId);
    }
}