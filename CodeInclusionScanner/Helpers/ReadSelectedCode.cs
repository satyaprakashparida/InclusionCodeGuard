using EnvDTE;

namespace CodeInclusionScanner.Helpers
{
    public class ReadSelectedCode
    {
        public static string Read()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));

            if (dte != null && dte.ActiveDocument != null)
            {
                TextSelection selection = dte.ActiveDocument.Selection as TextSelection;

                if (selection != null && !selection.IsEmpty)
                {
                    return selection.Text;
                }
            }

            return null;
        }
    }
}
