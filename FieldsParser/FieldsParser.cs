using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FieldsParser
{
    public static List<Token> ParseLine(string line)
    {
        var result = new List<Token>();

        int i = 0;
        while (i < line.Length)
        {
            if (line[i] == '\"' || line[i] == '\'')
            {
                result.Add(ReadQuotedField(line, i));
                i = result.Last().GetIndexNextToToken();
            }
            else if (line[i] == ' ')
                i++;
            else
            {
                result.Add(ReadField(line, i));
                i = result.Last().GetIndexNextToToken();
            }
        }

        return result;
    }

    private static Token ReadField(string line, int startIndex)
    {
        string lineForSearch = line.Substring(startIndex);
        string result = lineForSearch.Split(new char[] { ' ', '\"', '\'' })[0];
        return new Token(result, startIndex, result.Length);
    }

    public static Token ReadQuotedField(string line, int startIndex)
    {
        var lineValue = new StringBuilder();
        var length = 1;
        for (int i = startIndex + 1; i < line.Length; i++)
        {
            length++;
            if (line[i] == line[startIndex] && line[i - 1] != '\\')
                break;
            else if (line[i - 1] == '\\' && line[i] == '\\')
                lineValue.Append(line[i]);
            else if (line[i] != '\\')
                lineValue.Append(line[i]);
        }
        return new Token(lineValue.ToString(), startIndex, length);
    }
}