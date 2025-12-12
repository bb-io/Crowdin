namespace Apps.Crowdin.Polling.Models
{
    public class TasksPollingMemory
    {
        public DateTime LastPollingTime { get; set; }
        public bool Triggered { get; set; }
    }
}
