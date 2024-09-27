namespace Server.Validation;

public class ReflecionHelper
{
    public static bool IsNullable(Type? type)
    {
        if (type is null)
            return false;

        var a = Nullable.GetUnderlyingType(type);
        return Nullable.GetUnderlyingType(type) != null;
    }
    
    public static bool IsList(Type? type)
    {
        if (type is null)
            return false;

        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }
}