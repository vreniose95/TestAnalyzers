﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAnalyzers.Extensions;

namespace TestAnalyzers.Analyzers
{
  using System.Collections.Immutable;
  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.CSharp;
  using Microsoft.CodeAnalysis.CSharp.Syntax;
  using Microsoft.CodeAnalysis.Diagnostics;

  [DiagnosticAnalyzer(LanguageNames.CSharp)]
  internal class GU0072AllTypesShouldBeInternal : DiagnosticAnalyzer
  {
    public const string DiagnosticId = "GU0072";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
      id: DiagnosticId,
      title: "All types should be internal.",
      messageFormat: "All types should be internal.",
      category: "AnalyzerCategory.Correctness",
      defaultSeverity: DiagnosticSeverity.Warning,
      isEnabledByDefault: false, //AnalyzerConstants.DisabledByDefault,
      description: "All types should be internal.",
      helpLinkUri: "HelpLink.ForId(DiagnosticId)");

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
      ImmutableArray.Create(Descriptor);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
      context.EnableConcurrentExecution();
      context.RegisterSyntaxNodeAction(Handle, SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration);
    }

    private static void Handle(SyntaxNodeAnalysisContext context)
    {
      //if (context.IsExcludedFromAnalysis())
      //{
      //  return;
      //}

      if (context.Node is TypeDeclarationSyntax typeDeclaration &&
          context.ContainingSymbol is ITypeSymbol)
      {
        if (typeDeclaration.Modifiers.TrySingle(
          x => x.IsKind(SyntaxKind.PublicKeyword), 
          out var modifier))
        {
          context.ReportDiagnostic(Diagnostic.Create(Descriptor, modifier.GetLocation()));
        }
      }
    }
  }
}