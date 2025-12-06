/*
    This class stores the (parsed) content a chapter (.md) file  
*/
using System.Runtime.CompilerServices;

public class Chapter
{
    public string Title { get; private set; }

    public Chapter(string Title, string fileContent)
    {
        this.Title = Title;
    }

    private void ParseContent()
    {

    }

}