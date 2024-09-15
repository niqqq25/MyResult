using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MyResult.SourceGenerator.Templates;

namespace MyResult.SourceGenerator;

[Generator]
public sealed class ResultSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource($"{ResultAttributeTemplate.Name}.g.cs", ResultAttributeTemplate.Generate());
            ctx.AddSource($"{UndefinedErrorTemplate.Name}.g.cs", UndefinedErrorTemplate.Generate());
        });

        var declarations = context.SyntaxProvider.ForAttributeWithMetadataName(
                ResultAttributeTemplate.FullName,
                static (node, _) => IsSyntaxTargetForGeneration(node),
                static (ctx, _) => GetResultSourceContext(ctx))
            .Where(static d => d.HasValue);

        context.RegisterSourceOutput(declarations, static (ctx, resultContext) => Execute(ctx, resultContext!.Value));
    }

    private static void Execute(SourceProductionContext context, ResultContext resultContext)
    {
        var sourceCode = ResultTemplate.Generate(resultContext);

        var typeParametersPostfix = resultContext.ErrorType.IsGeneric
            ? "`2"
            : resultContext.ValueType.HasValue
                ? "`1"
                : string.Empty;

        context.AddSource(
            $"{resultContext.Namespace}_{resultContext.Name}{typeParametersPostfix}.g.cs",
            sourceCode);
    }

    private static ResultContext? GetResultSourceContext(GeneratorAttributeSyntaxContext context)
    {
        var symbolInformation = (INamedTypeSymbol)context.TargetSymbol;

        // Nested types are not supported.
        if (symbolInformation.ContainingType is not null)
        {
            return null;
        }

        var attributeData = GetAttributeData(symbolInformation);
        var typeInformation = (TypeDeclarationSyntax)context.TargetNode;
        var containingNamespace = symbolInformation.ContainingNamespace;

        var dataTypeParameter = symbolInformation.TypeParameters.ElementAtOrDefault(0);
        var errorTypeParameter = symbolInformation.TypeParameters.ElementAtOrDefault(1);

        return new ResultContext(
            symbolInformation.Name,
            typeInformation.Kind().ToTypeSymbol(),
            string.Join(" ", typeInformation.Modifiers.Select(mod => mod.Text)),
            containingNamespace.IsGlobalNamespace
                ? null
                : symbolInformation.ContainingNamespace.ToDisplayString(),
            new ErrorType(
                errorTypeParameter?.Name ??
                attributeData.ErrorTypeSymbol?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) ??
                UndefinedErrorTemplate.FullName,
                attributeData.ErrorTypeSymbol?.TypeKind is TypeKind.Interface,
                errorTypeParameter is not null),
            dataTypeParameter is not null
                ? new ValueType(dataTypeParameter.Name)
                : null,
            symbolInformation.GetOverriddenToStringMethod() is not null,
            attributeData.HasImplicitConversion,
            attributeData.IsSerializable);
    }

    private static AttributeData GetAttributeData(INamedTypeSymbol namedTypeSymbol) // TODO
    {
        const int attributeArgsCount = 3;

        var attributeData = namedTypeSymbol.GetAttributes()
            .Single(a => a.AttributeClass?.Name == ResultAttributeTemplate.Name);

        var args = attributeData.ConstructorArguments;

        if (args.Length is not attributeArgsCount)
        {
            throw new InvalidOperationException(
                $"Expected attribute to have {attributeArgsCount} arguments. It has {args.Length}.");
        }

        var errorType = args[0];
        if (errorType.Kind is not TypedConstantKind.Type)
        {
            throw new FormatException($"Expected {nameof(errorType)} to be a Type. It is a {errorType.Kind}.");
        }

        var hasImplicitConversion = args[1];
        if (hasImplicitConversion.Kind is not TypedConstantKind.Primitive || hasImplicitConversion.Value is not bool)
        {
            throw new FormatException(
                $"Expected {nameof(hasImplicitConversion)} to be boolean. It is a {hasImplicitConversion.Kind}.");
        }

        var isSerializable = args[2];
        if (isSerializable.Kind is not TypedConstantKind.Primitive || isSerializable.Value is not bool)
        {
            throw new FormatException(
                $"Expected {nameof(isSerializable)} to be boolean. It is a {isSerializable.Kind}.");
        }

        return new AttributeData(
            errorType.Value is not null ? (INamedTypeSymbol)errorType.Value! : null,
            (bool)hasImplicitConversion.Value,
            (bool)isSerializable.Value);
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
    {
        var kind = node.Kind();

        return kind
            is SyntaxKind.RecordStructDeclaration
            or SyntaxKind.RecordDeclaration
            or SyntaxKind.ClassDeclaration
            or SyntaxKind.StructDeclaration;
    }

    private readonly record struct AttributeData(
        INamedTypeSymbol? ErrorTypeSymbol,
        bool HasImplicitConversion,
        bool IsSerializable)
    {
        public INamedTypeSymbol? ErrorTypeSymbol { get; } = ErrorTypeSymbol;

        public bool HasImplicitConversion { get; } = HasImplicitConversion;

        public bool IsSerializable { get; } = IsSerializable;
    }
}