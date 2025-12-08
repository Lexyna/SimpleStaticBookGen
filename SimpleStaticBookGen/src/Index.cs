using System.Text.RegularExpressions;

public class Index : IHTMLGenerator
{
    private List<Chapter> Chapters = new();

    public List<string> ChapterLayout = new();

    private IndexObj indexObj;

    private string path;

    public Index(IndexObj index, string path)
    {
        this.path = path;

        if (index.Book == null)
        {
            Console.WriteLine("No Content found in index.json.");
            return;
        }

        this.indexObj = index;

        for (int i = 0; i < index.Book.GetLength(0); i++)
        {
            ChapterLayout.Add(index.Book[i][0]);
        }
    }

    public string GenerateSite()
    {
        string site = "<!DOCTYPE html>\n";

        site += "<html>";

        site += Utils.GenerateHead("Table of Contents");

        site += GenerateBody();

        site += "</html>";
        return site;
    }

    public string GenerateNavBar()
    {
        string navBar = "<nav class=\"toc\">\n";

        navBar += "<div class=\"contents\">\n";

        navBar += "<h2><small>Table of Contents</small></h2>\n";

        navBar += "<ul>\n";

        for (int i = 0; i < ChapterLayout.Count; i++)
        {
            navBar += $"<li><a href=\"#{ChapterLayout[i]}\"><small>{Utils.ToRomanNumber(i + 1)}</small>{ChapterLayout[i]}</a></li>\n";
        }

        navBar += "</ul>\n";

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

        body += "<div class=\"page\">\n";
        body += "<article class=\"content\">\n";
        body += "<h1 class=\"title\">Table of Contents</h1>\n";
        body += "<div class=\"chapters\">\n";
        body += "<div class=\"row\">\n";

        (List<List<Chapter>> left, List<List<Chapter>> right) = SplitChapters(indexObj.Book);

        body += "<div class=\"left\">\n";

        for (int i = 0; i < left.Count; i++)
        {
            Chapter titleChapter = left[i][0];

            body += $"<h2><span class=\"romanNum\">{Utils.ToRomanNumber(i + 1)}.</span><a href=\"{titleChapter.Title}.html\" name=\"{titleChapter.Title.ToLower()}\">{titleChapter.Title}</a></h2>\n";
            body += "<ul>\n";
            for (int j = 1; j < left[i].Count; j++)
            {
                Chapter chapter = left[i][j];
                body += GenerateChapterContent(chapter, j);
            }
            body += "</ul>\n";
        }

        body += "</div>\n";

        body += "<div class=\"right\">\n";

        for (int i = 0; i < right.Count; i++)
        {
            Chapter titleChapter = right[i][0];

            body += $"<h2><span class=\"romanNum\">{Utils.ToRomanNumber(left.Count + i + 1)}.</span><a href=\"{titleChapter.Title}.html\" name=\"{titleChapter.Title.ToLower()}\">{titleChapter.Title}</a></h2>\n";
            body += "<ul>\n";
            for (int j = 1; j < right[i].Count; j++)
            {
                Chapter chapter = right[i][j];
                body += GenerateChapterContent(chapter, j);
            }
            body += "</ul>\n";
        }

        body += "</div>\n";

        body += "</div>\n";
        body += "</div>\n";

        body += "<footer>";

        body += $"<a class=\"next\" href=\"{ChapterLayout[0]}.html\">Next Chapter: \"{ChapterLayout[0]}\" &#8594;</a>";
        body += "A guide by Lexyna &#8212;";
        body += $"<a href=\"https://github.com/Lexyna/SimpleStaticBookGen\"> © 2025</a>";

        body += "</footer>";

        body += "</article>\n";
        body += "</div>\n";
        body += "</body>\n";
        return body;
    }

    private string GenerateChapterContent(Chapter chapter, int index)
    {
        string content = "<li>\n";

        content += $"<span class=\"num\">{index}.</span><a href=\"{chapter.Title}.html\">{chapter.Title}</a>";

        content += "</li>";
        return content;
    }

    private (List<List<Chapter>> left, List<List<Chapter>> right) SplitChapters(string[][] book)
    {
        List<List<Chapter>> left = new();
        List<List<Chapter>> right = new();

        int chapters = book.GetLength(0);

        List<int> weight = new();
        int totalWeight = 0;

        for (int i = 0; i < chapters; i++)
        {
            weight.Add(book[i].Length);
            totalWeight += book[i].Length;
        }

        int index = 0;
        int bestDiff = int.MaxValue;
        int leftSum = 0;

        for (int i = 0; i < chapters; i++)
        {
            leftSum += weight[i];
            int rightSum = totalWeight - leftSum;

            int diff = Math.Abs(leftSum - rightSum);
            if (diff < bestDiff)
            {
                bestDiff = diff;
                index = i;
            }
        }

        int idxLeft = 0;
        int idxRight = 0;

        for (int i = 0; i < chapters; i++)
        {
            if (i <= index)
            {
                left.Add(new List<Chapter>());
                for (int j = 0; j < book[i].Length; j++)
                {
                    Chapter ch = new Chapter(book[i][j], path);
                    Chapters.Add(ch);
                    left[idxLeft].Add(ch);
                }
                idxLeft++;
            }
            else
            {
                right.Add(new List<Chapter>());
                for (int j = 0; j < book[i].Length; j++)
                {
                    Chapter ch = new Chapter(book[i][j], path);
                    Chapters.Add(ch);
                    right[idxRight].Add(ch);
                }
                idxRight++;
            }
        }

        return (left, right);
    }

    public void CreateHTML()
    {
        Utils.CreateHTMLFile("", "index", GenerateSite());
        foreach (Chapter ch in Chapters)
            ch.CreateHTML();
    }

}