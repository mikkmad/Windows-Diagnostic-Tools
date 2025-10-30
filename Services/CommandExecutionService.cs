using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;

namespace Windows_Diagnostic_Tools_rewrite.Services;

public class CommandExecutionService
{
    private readonly RichTextBlock _outputBlock;
    private readonly string[] _allowedCommands;

    public CommandExecutionService(RichTextBlock outputBlock, string[] allowedCommands)
    {
        _outputBlock = outputBlock;
        _allowedCommands = allowedCommands;
    }

    public async Task<bool> ExecuteAsync(string command)
    {
        if (!IsCommandAllowed(command))
        {
            AddLogWithNewline($"Error: Command '{command}' is not allowed.", Colors.Red);
            return false;
        }

        try
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            if (process == null) return false;

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();

            if (!string.IsNullOrEmpty(error))
            {
                AddLogWithNewline(error.Trim(), Colors.Red);
            }

            if (!string.IsNullOrEmpty(output))
            {
                AddLogWithNewline(output.Trim(), Colors.White);
            }

            await process.WaitForExitAsync();

            return process.ExitCode == 0;
        }
        catch (Exception ex)
        {
            AddLogWithNewline($"Error: {ex.Message}", Colors.Red);
            return false;
        }
    }

    public async Task RunSequentialAsync(string[] commands, bool requiresReboot = false)
    {
        _outputBlock.Blocks.Clear();
        AddLogWithNewline("Starting repair process...", Colors.White);

        foreach (var command in commands)
        {
            AddLogWithNewline($"Running: {command}", Colors.Cyan);
            AddLog(new string('=', 50), Colors.Gray);

            var success = await ExecuteAsync(command);

            if (!success)
            {
                AddLogWithNewline("Command failed. Stopping repair process.", Colors.Red);
                return;
            }

            AddLogWithNewline("Command completed successfully.", Colors.LimeGreen);
            AddLogWithNewline("", Colors.White);
        }

        AddLogWithNewline("Repair process completed successfully.", Colors.LimeGreen);

        if (requiresReboot)
        {
            AddLogWithNewline("", Colors.White);
            AddLogWithNewline("Please reboot your PC to complete the repair.", Colors.Yellow);
        }
    }

    private bool IsCommandAllowed(string command)
    {
        return _allowedCommands.Any(allowed =>
            allowed.Equals(command, StringComparison.OrdinalIgnoreCase));
    }

    private void AddLog(string text, Color color)
    {
        var paragraph = new Paragraph();
        var run = new Run { Text = text, Foreground = new SolidColorBrush(color) };
        paragraph.Inlines.Add(run);
        _outputBlock.Blocks.Add(paragraph);
    }

    private void AddLogWithNewline(string text, Color color)
    {
        var paragraph = new Paragraph();
        var run = new Run { Text = text + "\n", Foreground = new SolidColorBrush(color) };
        paragraph.Inlines.Add(run);
        _outputBlock.Blocks.Add(paragraph);
    }

    public void LogError(string message)
    {
        AddLogWithNewline($"Fatal Error: {message}", Colors.Red);
    }
}