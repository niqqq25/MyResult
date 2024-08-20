namespace MyResult.SourceGenerator.Templates;

internal static class UndefinedErrorTemplate
{
    public const string Namespace = "MyResult";
    
    public const string Name = "UndefinedError";

    public const string FullName = $"{Namespace}.{Name}";
    
    public static string Generate()
    {
        return $$"""
               namespace {{Namespace}}
               {
                   // TODO add explanation
                   public sealed class {{Name}};
               }
               """;
    }
}