using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MyResult.SourceGenerator;

internal static class SymbolExtensions
{
    public static IMethodSymbol? GetOverriddenToStringMethod(this ITypeSymbol typeSymbol)
    {
        var toStringMethods = typeSymbol.GetMembers("ToString").OfType<IMethodSymbol>();

        foreach (var eachMethod in toStringMethods)
        {
            if (eachMethod is { IsOverride: false, IsVirtual: false })
            {
                continue;
            }

            if (eachMethod.Parameters.Length != 0)
            {
                continue;
            }
            
            if (typeSymbol.IsRecord && eachMethod.IsImplicitlyDeclared)
            {
                continue;
            }
            
            return eachMethod;
        }

        return null;
    }
    
    public static TypeSymbol ToTypeSymbol(this SyntaxKind kind)
    {
        return kind switch
        {
            SyntaxKind.ClassDeclaration => TypeSymbol.Class,
            SyntaxKind.StructDeclaration => TypeSymbol.Struct,
            SyntaxKind.RecordDeclaration => TypeSymbol.Record,
            SyntaxKind.RecordStructDeclaration => TypeSymbol.RecordStruct,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, string.Empty)
        };
    }
    
    public static string ToConstruct(this TypeSymbol kind)
    {
        return kind switch
        {
            TypeSymbol.Class => "class",
            TypeSymbol.Struct => "struct",
            TypeSymbol.Record => "record",
            TypeSymbol.RecordStruct => "record struct",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, string.Empty)
        };
    }
}
