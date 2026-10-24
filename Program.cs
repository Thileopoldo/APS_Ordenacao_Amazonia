namespace APS_Ordenacao_Amazonia;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FormPrincipal());
    }
}
