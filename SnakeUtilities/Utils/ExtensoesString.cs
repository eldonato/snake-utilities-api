namespace SnakeUtilities.Utils;

public static class ExtensoesString
{
    public static string ApenasDigitos(this string? str)
    {
        str ??= string.Empty;
        return new string(str.Where(char.IsDigit).ToArray());
    }
}