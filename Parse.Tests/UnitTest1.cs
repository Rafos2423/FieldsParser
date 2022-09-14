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

    [TestCase("text", new[] { "text" })] // ���� ����
    [TestCase("hello world", new[] { "hello", "world" })] // ������ ������ ����, ����������� ������ � ���� ������
    [TestCase("hello \"world\"", new[] { "hello", "world" })] // ������� ���� ����� ���� � ��������, ���� � ������� ����� �������� ����
    [TestCase("\"he'l'lo\" world", new[] { "he'l'lo", "world" })] // ��������� ������� ������ �������
    [TestCase("'he\"l\"lo' world", new[] { "he\"l\"lo", "world" })] // ������� ������� ������ ���������
    [TestCase("\"he llo\" world", new[] { "he llo", "world" })] // ������ ������ �������
    [TestCase("'' world", new[] { "", "world" })] // ������ ����
    [TestCase("'hello'world", new[] { "hello", "world" })] // ����������� ��� ��������
    [TestCase("'he\"llo' world", new[] { "he\"llo", "world" })] // ���� � �������� ����� �������� ����
    [TestCase(" hello world ", new[] { "hello", "world" })] // ������� � ������ ��� � ����� ������
    [TestCase(@"'he\'l\'lo' world", new[] { @"he'l'lo", "world" })] // �������������� ��������� ������� ������ ���������
    [TestCase("hello  world", new[] { "hello", "world" })] // ����������� ������ >1 �������
    [TestCase("\"he\\\"l\\\"lo\" world", new[] { "he\"l\"lo", "world" })] // �������������� ������� ������� ������ �������
    [TestCase("'world ", new[] { "world " })] // ��� ����������� �������, ������ � ����� ���� � ���������� ��������
    [TestCase("", new string[0])] // ��� �����
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