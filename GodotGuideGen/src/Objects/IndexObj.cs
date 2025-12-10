/*
    This class contains the the index structure of the book as parsed by the index.json file
*/
public class IndexObj
{
    public string ProjectName { get; set; } = "";

    public string[][]? Book { get; set; }

}