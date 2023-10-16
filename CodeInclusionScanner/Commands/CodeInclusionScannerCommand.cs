using CodeInclusionScanner.Helpers;
using CodeInclusionScanner.Services;
using EnvDTE;

namespace CodeInclusionScanner
{
    [Command(PackageIds.CodeInclusionScannerCommand)]
    internal sealed class CodeInclusionScannerCommand : BaseCommand<CodeInclusionScannerCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            //var generatedResponse = await new Gpt3Service().GenerateAsync(ReadSelectedCode.Read());
            //await VS.MessageBox.ShowWarningAsync("CodeInclusionScanner", generatedResponse);

            var generatedResponse = await new AzureOpenAIService().ExtractNonInclusiveTerms(ReadSelectedCode.Read());
            await VS.MessageBox.ShowWarningAsync("CodeInclusionScanner", generatedResponse);
        }
    }
}
