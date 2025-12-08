using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

public class SSBQuoteBlockRenderer : HtmlObjectRenderer<QuoteBlock>
{
    protected override void Write(HtmlRenderer renderer, QuoteBlock obj)
    {
        renderer.WriteLine("<div class=\"quote\">");
        renderer.EnsureLine();
        renderer.WriteChildren(obj);
        renderer.EnsureLine();
        renderer.WriteLine("</div>");
    }
}