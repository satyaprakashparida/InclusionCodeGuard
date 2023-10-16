using System;
using System.Collections.Immutable;
using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class CodeAnalyzerAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor NonInclusiveWordUsed = new DiagnosticDescriptor(
        id: "NonInclusiveCodeAnalyzer001",
        title: "Noninclusive word usage",
        messageFormat: "Code contains non-inclusive term '{0}'.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly string[] NonInclusiveWords = new string[] { "blackman", "whiteman", "bad" }; // Add your non-inclusive words here

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(NonInclusiveWordUsed);

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSyntaxTreeAction(AnalyzeSyntaxLiteral);
        context.RegisterSyntaxTreeAction(AnalyzeSyntaxIdentifier);
        context.RegisterSyntaxNodeAction(AnalyzeSyntaxVariable, SyntaxKind.VariableDeclarator);
    }

    private void AnalyzeSyntaxLiteral(SyntaxTreeAnalysisContext context)
    {
        var root = context.Tree.GetRoot();

        foreach (var node in root.DescendantNodesAndSelf())
        {
            if (node is LiteralExpressionSyntax literalSyntax)
            {
                var literalText = literalSyntax.Token.Text;

                foreach (var nonInclusiveWord in NonInclusiveWords)
                {
                    if (literalText.Contains(nonInclusiveWord))
                    {
                        var diagnostic = Diagnostic.Create(
                            NonInclusiveWordUsed,
                            literalSyntax.GetLocation(),
                            nonInclusiveWord);

                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }

    private void AnalyzeSyntaxVariable(SyntaxNodeAnalysisContext context)
    {
        var variableDeclarator = (VariableDeclaratorSyntax)context.Node;
        var variableName = variableDeclarator.Identifier.Text;

        // Check if the variable name contains non-inclusive language.
        foreach (var nonInclusiveWord in NonInclusiveWords)
        {
            if (variableName.Contains(nonInclusiveWord))
            {
                var diagnostic = Diagnostic.Create(
                    NonInclusiveWordUsed,
                    variableDeclarator.GetLocation(),
                    nonInclusiveWord);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }

    private void AnalyzeSyntaxIdentifier(SyntaxTreeAnalysisContext context)
    {
        var root = context.Tree.GetRoot();

        foreach (var node in root.DescendantNodesAndSelf())
        {
            if (node is IdentifierNameSyntax identifierSyntax)
            {
                var identifierName = identifierSyntax.Identifier.Text;

                foreach (var pattern in NonInclusiveWords)
                {
                    if (identifierName.Contains(pattern))
                    {
                        var diagnostic = Diagnostic.Create(
                            NonInclusiveWordUsed,
                            identifierSyntax.GetLocation(),
                            pattern);

                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
