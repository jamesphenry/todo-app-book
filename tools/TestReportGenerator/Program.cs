using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;
using TestReportGenerator;

var inputFile = args.Length > 0 ? args[0] : "test-output.log";
var outputFile = "doc/test-results.md";

if (!File.Exists(inputFile))
{
    Console.Error.WriteLine($"❌ Input log file not found: {inputFile}");
    return;
}

var lines = File.ReadAllLines(inputFile);
var results = new List<SpecResult>();

var regex = new Regex(@"\[SpecResult\] (.+?) took (\d+) ms(?: \| Category=(.+?))?(?: \| Status=(Fail))?", RegexOptions.Compiled);

foreach (var line in lines)
{
    var match = regex.Match(line);
    if (!match.Success) continue;

    results.Add(new SpecResult
    {
        Name = match.Groups[1].Value,
        TimeMs = int.Parse(match.Groups[2].Value),
        Category = match.Groups[3].Success ? match.Groups[3].Value : "Uncategorized",
        Status = match.Groups[4].Success ? "Fail" : "Pass"
    });
}

var markdown = MarkdownReportGenerator.GenerateMarkdown(results);

Directory.CreateDirectory("doc");
File.WriteAllText(outputFile, markdown);

Console.WriteLine($"✅ Markdown test report written to {outputFile}");
