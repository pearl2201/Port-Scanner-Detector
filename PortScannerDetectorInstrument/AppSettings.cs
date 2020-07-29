using System.Collections.Generic;

public class AppSettings
{
    public string DstIp { get; set; }
    public List<string> IgnoreIps { get; set; }

    public int Duration { get; set; }

    public string ServerUri { get; set; }

    public string[] ReportOptions { get; set; }

    public string Strategy { get; set; }

    public int IndexDevice {get;set;}
}