using NUnit.Framework;

[TestFixture]
public class FieldsParserTests
{
    [TestCase("''", 0, "", 2)]
    [TestCase("'a'", 0, "a", 3)]
    [TestCase("\"abc\"", 0, "abc", 5)]
    [TestCase("b \"a'\"", 2, "a'", 4)]
    [TestCase(@"'a\' b'", 0, "a' b", 7)]
    [TestCase("'", 0, "", 1)]
    [TestCase("\"", 0, "", 1)]
    [TestCase(@"'a\' b\'", 0, "a' b'", 8)]
    [TestCase(@"'a\' b'xx", 0, "a' b", 7)]
    [TestCase(@"'\'\''xx", 0, "''", 6)]
    [TestCase("'a\"\"'", 0, "a\"\"", 5)]
    [TestCase("\"abc\"asas", 0, "abc", 5)]
    [TestCase("sx\"a'\"", 2, "a'", 4)]
    [TestCase("'a\"\"'", 0, "a\"\"", 5)]
    [TestCase("'a' b'", 0, "a", 3)]
    [TestCase("'a", 0, "a", 2)]
    public void TestQuotedField(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = FieldsParser.ReadQuotedField(line, startIndex);
        Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
    }

    [TestCase("text", new[] { "text" })] // Одно поле
    [TestCase("hello world", new[] { "hello", "world" })] // Больше одного поля, Разделитель длиной в один пробел
    [TestCase("hello \"world\"", new[] { "hello", "world" })] // Простое поле после поля в кавычках, Поле в кавыках после простого поля
    [TestCase("\"he'l'lo\" world", new[] { "he'l'lo", "world" })] // Одинарные кавычки внутри двойных
    [TestCase("'he\"l\"lo' world", new[] { "he\"l\"lo", "world" })] // Двойные кавычки внутри одинарных
    [TestCase("\"he llo\" world", new[] { "he llo", "world" })] // Пробел внутри кавычек
    [TestCase("'' world", new[] { "", "world" })] // Пустое поле
    [TestCase("'hello'world", new[] { "hello", "world" })] // Разделитель без пробелов
    [TestCase("'he\"llo' world", new[] { "he\"llo", "world" })] // Поле в кавычках после простого поля
    [TestCase(" hello world ", new[] { "hello", "world" })] // Пробелы в начале или в конце строки
    [TestCase(@"'he\'l\'lo' world", new[] { @"he'l'lo", "world" })] // Экранированные одинарные кавычки внутри одинарных
    [TestCase("hello  world", new[] { "hello", "world" })] // Разделитель длиной >1 пробела
    [TestCase("\"he\\\"l\\\"lo\" world", new[] { "he\"l\"lo", "world" })] // Экранированные двойные кавычки внутри двойных
    [TestCase("'world ", new[] { "world " })] // Нет закрывающей кавычки, Пробел в конце поля с незакрытой кавычкой
    [TestCase("", new string[0])] // Нет полей
    public static void RunTests(string input, string[] expectedResult)
    {
        var actualResult = FieldsParser.ParseLine(input);
        Assert.AreEqual(expectedResult.Length, actualResult.Count);
        for (int i = 0; i < expectedResult.Length; ++i)
        {
            Assert.AreEqual(expectedResult[i], actualResult[i].Value);
        }
    }
}