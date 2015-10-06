using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace DaveGrohlAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DaveGrohlAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DaveGrohlAnalyzer";

        internal const string Title = "Fight the foo.";
        internal const string MessageFormat = "Hey dummy, don't use foo as a variable name.";
        internal const string Category = "Naming";
        internal const string Description = "Find lazy method names";

        private static DiagnosticDescriptor Rule
            = new DiagnosticDescriptor(
                DiagnosticId,
                Title,
                MessageFormat,
                Category,
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            context.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);
        }

        private static void AnalyzeMethod(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (IMethodSymbol)context.Symbol;

            if (namedTypeSymbol.Name.ToLower() == "foo")
            {
                // For all such symbols, produce a diagnostic.

                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}