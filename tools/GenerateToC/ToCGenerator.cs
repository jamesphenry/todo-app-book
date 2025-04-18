using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestReportGenerator;

public class ToCGenerator
{
    public static void GenerateToC(string docDir, string coverFile)
    {
        var toc = new List<string>();
        var fileNames = Directory.GetFiles(docDir, "*.md");

        foreach (var fileName in fileNames)
        {
            var content = File.ReadAllText(fileName);
            var matches = Regex.Matches(content, @"^(#+)\s(.+)$", RegexOptions.Multiline);

            var fileToc = new List<string>();
            foreach (Match match in matches)
            {
                var level = match.Groups[1].Value.Length; // Number of '#' to determine the level
                var title = match.Groups[2].Value;
                var link = title.ToLower().Replace(" ", "-");

                // Indent nested items based on their header level
                var indent = new string(' ', (level - 1) * 2);
                fileToc.Add($"{indent}- [{title}](./{Path.GetFileName(fileName)}#{link})");
            }

            toc.AddRange(fileToc);
        }

        var tocContent = "# Table of Contents\n\n" + string.Join("\n", toc);
        var coverContent = File.ReadAllText(coverFile);
        var coverWithToc = coverContent.Replace("## Table of Contents", tocContent);

        File.WriteAllText(coverFile, coverWithToc);
    }
}
