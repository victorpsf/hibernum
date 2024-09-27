using System.Text.RegularExpressions;

namespace Server.Extensions;

public static class PrimitiveExtension
{
    public static string ToLike(this string value)
        => $"%{Regex.Replace(value, "[^\\w]+", "%", RegexOptions.ECMAScript)}%";
}