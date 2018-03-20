using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;

namespace TestAnalyzers.Analyzers
{
  [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MakeIntegralTypeConstraintFixProvider))]
  [Shared]
  internal class MakeIntegralTypeConstraintFixProvider
    : CodeFixProvider
  {
    /// <inheritdoc/>
    public override ImmutableArray<string> FixableDiagnosticIds { get; } 
      = ImmutableArray.Create(GU0073AllUsagesOfIntegralTypeParameterConstraintShouldBeIntegral.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc/>
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
      var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken)
                                    .ConfigureAwait(false);

      foreach (var diagnostic in context.Diagnostics)
      {
        var token = syntaxRoot.FindToken(
          diagnostic.Location.SourceSpan.Start);

        if (token.IsKind(SyntaxKind.PublicKeyword))
        {
          context.RegisterCodeFix(
            CodeAction.Create(
              "Make internal.",
              _ => Task.FromResult(
                context.Document.WithSyntaxRoot(
                  syntaxRoot.ReplaceToken(
                    token,
                    SyntaxFactory.Token(SyntaxKind.InternalKeyword)))),
              nameof(MakeIntegralTypeConstraintFixProvider)),
            diagnostic);
        }
      }
    }
  }
}
