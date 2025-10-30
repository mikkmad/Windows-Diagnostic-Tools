using System;
using Microsoft.UI.Xaml;
using Windows_Diagnostic_Tools_rewrite.Services;

namespace Windows_Diagnostic_Tools_rewrite.Views;

public sealed partial class WindowsRepair
{
    private static readonly string[] AllowedCommands =
    [
        "sfc /scannow",
        "DISM /Online /Cleanup-Image /RestoreHealth"
    ];

    private readonly CommandExecutionService _commandService;

    public WindowsRepair()
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