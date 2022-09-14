using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Programm
{
    public static void Main(string[] args)
    {
        List<Token> result = FieldsParser.ParseLine("\"bcd ef\" a 'x y'");

        foreach(var token in result)
            Console.WriteLine(token.ToString());
    }
}
