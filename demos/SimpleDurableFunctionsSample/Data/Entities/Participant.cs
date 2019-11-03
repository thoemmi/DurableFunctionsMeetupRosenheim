namespace SimpleDurableFunctionsSample.Data.Entities
{
    public class Participant
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public int WorkflowId { get; set; }

        public Workflow Workflow { get; set; }
    }
}