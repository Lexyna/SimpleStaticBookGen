using System.Runtime.CompilerServices;
using System.Text;

public static class Utils
{

    private static string SITE = "Site";

    public static string ToRomanNumber(int number)
    {
        StringBuilder result = new StringBuilder();
        int[] digitsValues = { 1, 4, 5, 9, 10, 40, 50, 90, 100, 400, 500, 900, 1000 };
        string[] romanDigits = { "I", "IV", "V", "IX", "X", "XL", "L", "XC", "C", "CD", "D", "CM", "M" };
        while (number > 0)
        {
            for (int i = digitsValues.Count() - 1; i >= 0; i--)
                if (number / digitsValues[i] >= 1)
                {
                    number -= digitsValues[i];
                    result.Append(romanDigits[i]);
                    break;
                }
        }
        return result.ToString();
    }

    public static string GenerateHead(string title)
    {
        string head = "<head>\n";

        head += "<meta http-equiv=\"Content-type\" content=\"text/html;charset=UTF-8\">\n";
        head += $"<title>{title}</title>\n";
        head += "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\n";
        //Google fonts
        head += "<link rel=\"preconnect\" href=\"https://fonts.googleapis.com\">\n";
        head += "<link rel=\"preconnect\" href=\"https://fonts.gstatic.com\" crossorigin>\n";
        head += "<link href=\"https://fonts.googleapis.com/css2?family=Bitcount+Prop+Single:wght@100..900&display=swap\" rel =\"stylesheet\">\n";

        //head += "<link rel=\"icon\" type=\"image/png\" href=\"image/favicon.png\">\n";

        head += "<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.11.1/styles/default.min.css\">\n";
        head += "<link rel=\"stylesheet\" href=\"https://cdn.jsdelivr.net/npm/@catppuccin/highlightjs@1.0.1/css/catppuccin-macchiato.css\">\n";
        head += "<script src=\"https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.11.1/highlight.min.js\"></script>\n";

        // TODO: Provide gdscript 4.x extension for highlight.js officially
        head += "<script src=\"gdscript.js\"></script>\n";

        head += "<script>hljs.registerLanguage(\"gdscript\", GDScript);hljs.highlightAll();</script>";
        head += "<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\">\n";



        head += "</head>\n";
        return head;
    }

    public static void CreateHTMLFile(string path, string name, string html)
    {
        if (!Directory.Exists(path + $"/{SITE}"))
            Directory.CreateDirectory(path + $"/{SITE}");

        File.WriteAllText(path + $"/{SITE}/{name}.html", html);

    }

}