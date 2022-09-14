public class Token
{
    public readonly int Length;
    public readonly int Position;
    public readonly string Value;

    public Token(string value, int position, int length)
    {
        Position = position;
        Length = length;
        Value = value;
    }

    public int GetIndexNextToToken()
    {
        return Position + Length;
    }

    public override string ToString()
    {
        var value = $"[{Value}]";
        return $"{value.PadRight(10)} Position={Position:##0} \t Length={Length}";
    }
}