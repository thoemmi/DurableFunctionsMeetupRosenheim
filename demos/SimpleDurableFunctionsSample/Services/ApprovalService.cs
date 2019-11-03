using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleDurableFunctionsSample.Data;
using SimpleDurableFunctionsSample.Data.Entities;
using SimpleDurableFunctionsSample.Models;

namespace SimpleDurableFunctionsSample.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly WorkflowContext _context;

        public ApprovalService(WorkflowContext context)
        {
            _context = context;
        }

        public async Task<Workflow> Create(WorkflowBaseData workflowBaseData)
        {
            var workflow = new Workflow()
            {
                InstanceId = workflowBaseData.InstanceId,
                CreatedAt = DateTime.Now,
                Status = WorkflowStatus.Created.ToString(),
                Participants = workflowBaseData.Participants?.Select(m => new Participant()
                {
                    Name = m,
                    Status = ParticipantStatus.Waiting.ToString()
                }).ToList()
            };

            var result = await _context.Workflows.AddAsync(workflow);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task SetStatus(int workflowId, WorkflowStatus status)
        {
            var workflow = await _context.Workflows.FirstOrDefaultAsync(m => m.Id == workflowId);
            if (workflow != null)
            {
                workflow.Status = status.ToString();
                await _context.SaveChangesAsync();
            }
        }

        public async Task Approve(int workflowId, string userName)
        {
            var user = await _context.Participants.FirstOrDefaultAsync(m => m.WorkflowId == workflowId && m.Name == userName);
            if (user != null && user.Status == ParticipantStatus.Waiting.ToString())
            {
                user.Status = ParticipantStatus.Approved.ToString();
                await _context.SaveChangesAsync();
            }
        }

        public async Task Deny(int workflowId, string userName)
        {
            var user = await _context.Participants.FirstOrDefaultAsync(m => m.WorkflowId == workflowId && m.Name == userName);
            if (user != null && user.Status == ParticipantStatus.Waiting.ToString())
            {
                user.Status = ParticipantStatus.Denied.ToString();
                await _context.SaveChangesAsync();
            }
        }

        public Task<bool> IsParticipantInWaitingState(int workflowId)
        {
            return _context.Participants
                .Where(m => m.WorkflowId == workflowId)
                .AnyAsync(m => m.Status == ParticipantStatus.Waiting.ToString());
        }

        public async Task<string> GetInstanceId(int workflowId)
        {
            var workflow = await _context.Workflows
                .FirstOrDefaultAsync(m => m.Id == workflowId);

            return workflow?.InstanceId;
        }
    }
}