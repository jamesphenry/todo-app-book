namespace TestReportGenerator;

public class SpecResult
{
    public string Name { get; set; } = "";
    public int TimeMs { get; set; }
    public string Category { get; set; } = "Uncategorized";
    public string Status { get; set; } = "Pass";
}
