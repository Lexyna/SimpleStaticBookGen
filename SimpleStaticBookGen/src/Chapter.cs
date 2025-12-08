/*
    This class stores the (parsed) content a chapter (.md) file  
*/
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

public class Chapter : IHTMLGenerator
{
    public string Title { get; private set; }

    private string Content { get; set; }

    private MarkdownDocument document;

    private bool initialized = false;

    public Chapter(string Title, string path)
    {
        this.Title = Title;

        if (!Directory.Exists(path)) return;

        if (!File.Exists(path + $"/{this.Title}.md")) return;

        string content = File.ReadAllText(path + $"/{this.Title}.md");

        ParseContent(content);
        initialized = true;
    }

    private void ParseContent(string fileContent)
    {
        var pipeline = new MarkdownPipelineBuilder().Build();

        var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);

        renderer.ObjectRenderers.RemoveAll(r => r is QuoteBlockRenderer);
        renderer.ObjectRenderers.Add(new SSBQuoteBlockRenderer());

        pipeline.Setup(renderer);

        document = Markdown.Parse(fileContent, pipeline);
        renderer.Render(document);

        Content = writer.ToString();
    }

    public string GenerateNavBar()
    {
        var headings = document.Descendants<HeadingBlock>().Select(h => (h.Level, Text: h.Inline?.FirstChild?.ToString() ?? "")).ToList();

        string navBar = "<nav class=\"toc\">\n";
        navBar += "<div class=\"contents\">\n";
        navBar += $"<h2><small>{Title}</small></h2>\n";
        navBar += "<ul>";

        for (int i = 1; i < headings.Count; i++)
        {
            if (headings[i].Level >= 3) continue;

            navBar += $"<li><a href=\"\"><small>{i}</small>{headings[i].Text}</a></li>";

        }

        navBar += "</ul>";


        navBar += "<div class =\"prev-next\">\n";

        string prevTitle = "";
        string nextTitle = "";

        navBar += $"<a class=\"prev\" href=\"{prevTitle}.html\">&#8592; Prev</a>\n";
        navBar += $"<a class=\"next\" href=\"{nextTitle}.html\">Next &#8594;</a>\n";

        navBar += "</div>\n";

        navBar += "</div>\n";
        navBar += "</nav>\n";
        return navBar;
    }

    public string GenerateBody()
    {
        string body = "<body>\n";

        body += GenerateNavBar();


        body += "<body>\n";
        return body;
    }

    public string GenerateSite()
    {
        string site = "<!DOCTYPE html>\n";
        site += "<html>";

        site += Utils.GenerateHead(Title);

        site += GenerateBody();

        site += "</html>";
        return site;
    }

    public void CreateHTML()
    {
        if (!initialized)
            return;
        Utils.CreateHTMLFile("", Title, GenerateSite());

    }
}