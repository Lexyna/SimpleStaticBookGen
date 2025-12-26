using System.Text.RegularExpressions;

public class ScriptObj
{
  public class SnippetIdx
  {
    public int start { get; private set; }
    public int end { get; private set; }
    public SnippetIdx(int start)
    {
      this.start = start;
    }

    public void SetEndIdx(int end)
    {
      this.end = end;
    }

  }

  private static string patternStart = "#>>\\s*([a-zA-Z0-9-_]+\\s?)+";
  private static string patternEnd = "#<<\\s*([a-zA-Z0-9-_]+\\s?)+";

  private List<string> Lines = new();

  public Dictionary<string, (List<string>, SnippetIdx)> blocks = new();

  public string FileName { get; private set; }

  private string RelativePath;

  private HashSet<string> currentBlocks = new();

  public ScriptObj(string path, string relativePath, string fileName)
  {
    this.FileName = fileName;
    this.RelativePath = relativePath;
    LoadScript(path);
  }

  private void LoadScript(string path)
  {
    string fullPath = Path.Combine(path, RelativePath);

    if (!File.Exists(fullPath)) return;

    var pre_lines = File.ReadAllLines(fullPath).ToList();

    int lineIdx = 1;

    for (int i = 0; i < pre_lines.Count; i++)
    {
      var line = pre_lines[i];

      Match startBlock = Regex.Match(line, patternStart);
      Match endBlock = Regex.Match(line, patternEnd);

      if (startBlock.Success)
      {
        MatchCollection matches = Regex.Matches(line, patternStart);
        for (int j = 0; j < matches[0].Groups[1].Captures.Count; j++)
        {

          string id = matches[0].Groups[1].Captures[j].Value.Trim();

          if (!currentBlocks.Contains(id))
            currentBlocks.Add(id);

        }

        continue;
      }

      if (endBlock.Success)
      {
        MatchCollection matches = Regex.Matches(line, patternEnd);
        for (int j = 0; j < matches[0].Groups[1].Captures.Count; j++)
        {

          string id = matches[0].Groups[1].Captures[j].Value.Trim();

          if (currentBlocks.Contains(id))
            currentBlocks.Remove(id);

        }

        continue;
      }

      Lines.Add(line);

      foreach (var block in currentBlocks)
      {
        if (!blocks.ContainsKey(block))
          blocks.Add(block, (new List<string>(), new SnippetIdx(lineIdx)));
        blocks[block].Item1.Add(line);
        blocks[block].Item2.SetEndIdx(lineIdx);
      }

      lineIdx++;

    }
  }

  public string GenerateCodeSnippet(string blockId, int before, int after, string displayText)
  {
    string snippet = "";

    int start = blocks[blockId].Item2.start;
    int end = blocks[blockId].Item2.end;

    snippet += "<div class=\"snippet\">\n";
    if (before > 0)
      snippet += SnippetBefore(start, before);
    snippet += SnippetMain(blockId, displayText);
    if (after > 0)
      snippet += SnippetAfter(end, after);

    snippet += "</div>\n";

    snippet += "<div class=\"source-btm\">\n";
    snippet += $"<em>{RelativePath}</em>";
    if (displayText != null)
      snippet += " " + displayText;
    snippet += "</div>\n";

    return snippet;
  }

  private string SnippetMain(string blockId, string displayText)
  {
    string snippet = "";

    snippet += "<div class=\"source\">\n";
    snippet += $"<em>{RelativePath}</em><br>\n";

    if (displayText != null)
      snippet += displayText;

    snippet += "</div>";

    List<string> lines = blocks[blockId].Item1;

    snippet += "<pre class=\"snippet-main\">\n";
    snippet += "<code class=\"language-gdscript\">\n";

    for (int i = 0; i < lines.Count; i++)
    {
      snippet += lines[i] + "\n";
    }

    snippet += "</code>\n";
    snippet += "</pre>\n";

    return snippet;
  }

  private string SnippetBefore(int start, int before)
  {
    string snipperBefore = "";

    int startLine = start - before;

    if (startLine < 1 || startLine + before >= Lines.Count) return snipperBefore;

    snipperBefore += "<pre class=\"snippet-before\">\n";
    snipperBefore += "<code>\n";

    for (int i = startLine; i < startLine + before - 1; i++)
    {
      snipperBefore += Lines[i] + "\n";
    }

    snipperBefore += "</code>\n";
    snipperBefore += "</pre>\n";
    return snipperBefore;
  }

  private string SnippetAfter(int end, int after)
  {
    string snippetAfter = "";

    int endLine = end;

    if (endLine < 1 || endLine + after >= Lines.Count) return snippetAfter;

    snippetAfter += "<pre class=\"snippet-after\">\n";
    snippetAfter += "<code>\n";

    for (int i = endLine; i < endLine + after - 1; i++)
    {
      snippetAfter += Lines[i] + "\n";
    }

    snippetAfter += "</code>\n";
    snippetAfter += "</pre>\n";
    return snippetAfter;
  }

}