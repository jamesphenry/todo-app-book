using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Linq;

var docDir = Path.Combine("doc");
var coverFile = Path.Combine(docDir, "cover.md");

if (!File.Exists(coverFile))
{
    Console.WriteLine($"❌ Cover file not found at: {coverFile}");
    return;
}

Console.WriteLine($"📘 Generating ToC for docs in: {docDir}");

var toc = new List<string>();
var files = Directory.GetFiles(docDir, "*.md")
    .Where(f => !Path.GetFileName(f).Equals("cover.md", StringComparison.OrdinalIgnoreCase));

foreach (var file in files)
{
    var content = File.ReadAllText(file);
    var matches = Regex.Matches(content, @"^(#+)\s(.+)$", RegexOptions.Multiline);

    foreach (Match match in matches)
    {
        var level = match.Groups[1].Value.Length;
        var title = match.Groups[2].Value.Trim();
        var link = title.ToLower().Replace(" ", "-").Replace(":", "").Replace(".", "");
        var indent = new string(' ', (level - 1) * 2);
        toc.Add($"{indent}- [{title}]({Path.GetFileName(file)}#{link})");
    }
}

var tocBlock = "# Table of Contents\n\n" + string.Join("\n", toc);
var cover = File.ReadAllText(coverFile);
var newCover = Regex.Replace(cover, @"## Table of Contents.*?---", $"{tocBlock}\n\n---", RegexOptions.Singleline);

File.WriteAllText(coverFile, newCover);
Console.WriteLine("✅ ToC updated in cover.md!");
