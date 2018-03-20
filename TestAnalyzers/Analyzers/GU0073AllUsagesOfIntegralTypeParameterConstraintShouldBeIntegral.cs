using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using TestAnalyzers.Extensions;

namespace TestAnalyzers.Analyzers
{
  [DiagnosticAnalyzer(LanguageNames.CSharp)]
  public class GU0073AllUsagesOfIntegralTypeParameterConstraintShouldBeIntegral
  : DiagnosticAnalyzer
  {
    public const string DiagnosticId = "GU0072";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
      DiagnosticId,
      "All usages of integral type parameter constraint should be integral.",
      "All usages of integral type parameter constraint should be integral.",
      "AnalyzerCategory.Correctness",
      DiagnosticSeverity.Warning,
      true, //AnalyzerConstants.DisabledByDefault,
      "All usages of integral type parameter constraint should be integral.",
      $"HelpLink.ForId({DiagnosticId})");

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
      ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
      context.EnableConcurrentExecution();
      context.RegisterSyntaxNodeAction(
        Handle, 
        SyntaxKind.TypeConstraint);
    }

    private static void Handle(
      SyntaxNodeAnalysisContext context)
    {
      if (context.Node is TypeDeclarationSyntax typeDeclaration 
          && context.ContainingSymbol is ITypeSymbol)
      {
        if (typeDeclaration
            .Modifiers
            .TrySingle(
              t => t.IsKind(
                SyntaxKind.TypeArgumentList),
          out var modifier))
        {
          context.ReportDiagnostic(
            Diagnostic.Create(
              Descriptor, 
              modifier.GetLocation()));
        }
      }
    }
    
  }
}
