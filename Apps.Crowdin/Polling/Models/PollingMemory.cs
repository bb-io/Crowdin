namespace Apps.Crowdin.Polling.Models;

public class PollingMemory
{
    public DateTime? LastPollingTime { get; set; }

    public bool Triggered { get; set; }
}