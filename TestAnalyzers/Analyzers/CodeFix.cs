using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAnalyzers.Analyzers
{
  using Gu.Roslyn.Asserts;
  using NUnit.Framework;

  internal class CodeFix
  {
    private static readonly GU0072AllTypesShouldBeInternal Analyzer = new GU0072AllTypesShouldBeInternal();
    private static readonly MakeInternalFixProvider Fix = new MakeInternalFixProvider();
    private static readonly ExpectedDiagnostic ExpectedDiagnostic = ExpectedDiagnostic.Create("GU0072");

    [Test]
    public void Class()
    {
      var testCode = @"
namespace RoslynSandbox
{
    ↓public class Foo
    {
    }
}";

      var fixedCode = @"
namespace RoslynSandbox
{
    internal class Foo
    {
    }
}";
      AnalyzerAssert.CodeFix(Analyzer, Fix, ExpectedDiagnostic, testCode, fixedCode);
    }

    [Test]
    public void Struct()
    {
      var testCode = @"
namespace RoslynSandbox
{
    ↓public struct Foo
    {
    }
}";

      var fixedCode = @"
namespace RoslynSandbox
{
    internal struct Foo
    {
    }
}";
      AnalyzerAssert.CodeFix(Analyzer, Fix, ExpectedDiagnostic, testCode, fixedCode);
    }
  }
}
