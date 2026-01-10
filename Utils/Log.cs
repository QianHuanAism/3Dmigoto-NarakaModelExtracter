using System;
using System.Collections.Generic;
using System.Text;

namespace NMC.Utils;

public static class Log
{
    public static List<string> LogList { get; } = new List<string>();
    public static string TimeStamp { get; } = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

    public static void Info(string content)
    {
        LogList.Add($"[{TimeStamp} INFO] {content}");
    }

    public static void Err(string content)
    {
        LogList.Add($"[{TimeStamp} ERROR] {content}");
    }
}
