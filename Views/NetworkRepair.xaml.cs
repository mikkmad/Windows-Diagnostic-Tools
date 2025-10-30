using System;
using Microsoft.UI.Xaml;
using Windows_Diagnostic_Tools_rewrite.Services;

namespace Windows_Diagnostic_Tools_rewrite.Views;

public sealed partial class NetworkRepair
{
    private static readonly string[] AllowedCommands =
    [
        "ipconfig /flushdns",
        "ipconfig /release", 
        "ipconfig /renew",
        "ipconfig /registerdns",
        "netsh winsock reset"
    ];

    private readonly CommandExecutionService _commandService;

    public NetworkRepair()
    {
        InitializeComponent();
        _commandService = new CommandExecutionService(OutputRichTextBlock, AllowedCommands);
    }

    private async void RunCommand_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await _commandService.RunSequentialAsync(AllowedCommands, requiresReboot: true);
        }
        catch (Exception ex)
        {
            _commandService.LogError(ex.Message);
        }
    }
}