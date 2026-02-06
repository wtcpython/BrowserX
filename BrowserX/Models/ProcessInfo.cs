namespace BrowserX.Models;

public class ProcessInfo
{
    public string Task { get; set; }
    public long Memory { get; set; }
    public string MemoryKB { get; set; }
    public int ProcessId { get; set; }
}