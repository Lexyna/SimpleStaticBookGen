using System.Text.RegularExpressions;
using Markdig.Extensions.CustomContainers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

public class GGGCustomContainerRenderer : HtmlObjectRenderer<CustomContainer>
{
    //Matches the format: {id}:{num}:{num}:{text}
    private static string pattern = "([a-zA-Z0-9-_]+):([0-9]+):([0-9]+):?([a-zA-Z\\s*.-_']*)$";

    private static string ATTR_CODE = "code";

    protected override void Write(HtmlRenderer renderer, CustomContainer obj)
    {
        renderer.EnsureLine();

        if (obj.Info == ATTR_CODE)
        {
            //render custom html based on parsed scripts 
            renderer.Write(GenerateScriptBlock(obj));

            return;
        }

        if (renderer.EnableHtmlForBlock)
        {
            renderer.Write("<div").WriteAttributes(obj).Write('>');
        }

        renderer.WriteChildren(obj);
        if (renderer.EnableHtmlForBlock)
        {
            renderer.WriteLine("</div>");
        }
    }

    private string GenerateScriptBlock(CustomContainer obj)
    {
        if (!(obj.LastChild is ParagraphBlock)) return "";

        var child = obj.LastChild as ParagraphBlock;

        var content = child?.Inline?.FirstChild?.ToString();

        if (content == null) return "";

        Match correctSignature = Regex.Match(content, pattern);

        if (!correctSignature.Success) return "";

        MatchCollection matches = Regex.Matches(content, pattern);

        int before = 0, after = 0;

        string snippetId = matches[0].Groups[1].Value;
        Int32.TryParse(matches[0].Groups[2].Value, out before);
        Int32.TryParse(matches[0].Groups[3].Value, out after);
        string displayInfo = matches[0].Groups[4].Value;

        return ScriptGenerator.CerateCodeSnippet(snippetId, before, after, displayInfo);
    }

}