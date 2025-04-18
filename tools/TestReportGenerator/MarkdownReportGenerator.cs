using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

namespace TestReportGenerator;

public static class MarkdownReportGenerator
{
    public static string GenerateMarkdown(List<SpecResult> results)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# 🧪 Test Report — TodoApp Specs\n");
        sb.AppendLine($"_Last run: {DateTime.UtcNow:yyyy-MM-dd}_\n");

        sb.AppendLine("| ✅ Status | 🧾 Spec Name | 🕒 Time (ms) | 🗂️ Category |");
        sb.AppendLine("|----------|--------------|-------------|--------------|");

        foreach (var r in results)
        {
            sb.AppendLine($"| {(r.Status == "Pass" ? "✅ Pass" : "❌ Fail")} | {r.Name} | {r.TimeMs} | {r.Category} |");
        }

        var fastest = results.OrderBy(r => r.TimeMs).FirstOrDefault();
        var slowest = results.OrderByDescending(r => r.TimeMs).FirstOrDefault();

        if (fastest != null && slowest != null)
        {
            sb.AppendLine($"\n> ⏱️ Fastest Test: `{fastest.Name}` ({fastest.TimeMs}ms)");
            sb.AppendLine($"> 🐢 Slowest Test: `{slowest.Name}` ({slowest.TimeMs}ms)");
        }

        sb.AppendLine("\n---\n### 📚 Summary");
        sb.AppendLine($"- Total: {results.Count}");
        sb.AppendLine($"- Passed: {results.Count(r => r.Status == "Pass")} ✅");
        sb.AppendLine($"- Failed: {results.Count(r => r.Status == "Fail")} ❌");

        sb.AppendLine("\n---\nGenerated from `[SpecResult]` logs by TestReportGenerator.");

        return sb.ToString();
    }
}
