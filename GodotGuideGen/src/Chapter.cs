/*
    This class stores the (parsed) content a chapter (.md) file  
*/
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Markdig;
using Markdig.Extensions.CustomContainers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

public class Chapter : IHTMLGenerator
{
    private string ProjectName { get; set; }
    public string FileName { get; private set; }
    public string? Title { get; private set; }

    private string? Content { get; set; }

    private MarkdownDocument? document;

    private string path = "";

    private bool initialized = false;

    private string? prev, next;
    private string? prevLink, nextLink;

    public Chapter(string projectName, string FileName, string path)
    {
        this.ProjectName = projectName;
        this.FileName = FileName;
        this.path = path;

        string fullPath = Path.Combine(path, "Book", $"{this.FileName}.md");

        if (!File.Exists(fullPath)) return;

        string content = File.ReadAllText(fullPath);

        Content = ParseContent(content);
        initialized = true;
    }

    public string GetPath()
    {
        return FileName;
    }

    public void LinkChapters(string prev, string prevLink, string next, string nextLink)
    {
        this.prev = prev;
        this.prevLink = prevLink;
        this.next = next;
        this.nextLink = nextLink;
    }

    private string ParseContent(string fileContent)
    {
        var pipeline = new MarkdownPipelineBuilder().UsePipeTables().UseCustomContainers().Build();

        var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);

        pipeline.Setup(renderer);

        renderer.ObjectRenderers.RemoveAll(r => r is HeadingRenderer);
        renderer.ObjectRenderers.Add(new GGGHeadingRenderer());

        renderer.ObjectRenderers.RemoveAll(r => r is HtmlCustomContainerRenderer);
        renderer.ObjectRenderers.Add(new GGGCustomContainerRenderer());

        document = Markdown.Parse(fileContent, pipeline);
        renderer.Render(document);

        var headings = document.Descendants<HeadingBlock>().Select(h => (h.Level, Text: h.Inline?.FirstChild?.ToString() ?? "")).ToList();

        this.Title = headings[0].Text;

        return writer.ToString();
    }

    public string GenerateNavBar()
    {
        var headings = document?.Descendants<HeadingBlock>().Select(h => (h.Level, Text: h.Inline?.FirstChild?.ToString() ?? "")).ToList();

        string navBar = "<nav class=\"toc\">\n";
        navBar += "<div class=\"contents\">\n";
        navBar += $"<h1>{ProjectName}</h1>\n";
        navBar += $"<h2><small>{Title}</small></h2>\n";
        navBar += "<ul>";

        int index = 1;

        for (int i = 1; i < headings?.Count; i++)
        {
            if (headings[i].Level >= 3) continue;

            string titleId = headings[i].Text.Replace(" ", "-");

            navBar += $"<li><a href=\"#{titleId}\"><small>.{Utils.ToRomanNumber(index)}</small>{headings[i].Text}</a></li>";
            index++;

        }

        navBar += "</ul>";


        navBar += "<div class =\"prev-next\">\n";

        string prevTitle = prevLink != null ? prevLink : "";
        string nextTitle = nextLink != null ? nextLink : "";

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
        body += "<div class=\"page\">\n";

        body += "<article class=\"chapter\">\n";

        body += Content;

        body += "<footer>";

        body += $"<a class=\"next\" href=\"{nextLink}.html\">Next Chapter: \"{next}\" &#8594;</a>";
        body += "A guide by Lexyna &#8212;";
        body += $"<a href=\"https://github.com/Lexyna/SimpleStaticBookGen\"> © 2025</a>";

        body += "</footer>";

        body += "</article>\n";
        body += "</div>\n";
        body += "<body>\n";
        return body;
    }

    public string GenerateSite()
    {
        string site = "<!DOCTYPE html>\n";
        site += "<html>";

        string title = Title != null ? Title : FileName;

        site += Utils.GenerateHead(this.ProjectName + " - " + title);

        site += GenerateBody();

        site += "</html>";
        return site;
    }

    public void CreateHTML()
    {
        if (!initialized)
            return;
        Utils.CreateHTMLFile(path, FileName, GenerateSite());
        Console.WriteLine($"Created {FileName}.html");

    }
}