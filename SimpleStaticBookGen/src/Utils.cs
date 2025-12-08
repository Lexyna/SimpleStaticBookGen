using System.Runtime.CompilerServices;
using System.Text;

public static class Utils
{

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

        head += "<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\">\n";
        //head += "<link rel=\"icon\" type=\"image/png\" href=\"image/favicon.png\">\n";

        head += "</head>\n";
        return head;
    }

    public static void CreateHTMLFile(string path, string name, string html)
    {
        if (!Directory.Exists("site"))
            Directory.CreateDirectory("site");

        File.WriteAllText($"site/{name}.html", html);

    }

}