namespace rinha_2024_q1;

public static class AssertionConcern
{
    public static void AssertRequired(object value, string message)
    {
        if (value is null)
        {
            throw new ArgumentException(message);
        }
    }

    public static void AssertLength(string value, int minimum, int maximum, string message)
    {
        var length = value.Length;
        if (length < minimum || length > maximum)
        {
            throw new ArgumentException(message);
        }
    }

    public static void AssertPositive(int value, string message)
    {
        if (value <= 0)
        {
            throw new ArgumentException(message);
        }
    }
}
