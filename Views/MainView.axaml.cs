using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;

namespace Windows_Diagnostic_Tools.Views;

public partial class MainView : UserControl
{
    private TextBox _outputTextBoxNetwork;
    private TextBox _outputTextBoxRepair;

    public MainView()
    {
        InitializeComponent();
        _outputTextBoxNetwork = this.FindControl<TextBox>("NetworkAndDnsOutputTextbox");
        _outputTextBoxRepair  = this.FindControl<TextBox>("RepairWindowsImageOutputTextbox");
    }

    private async void ResetNetworkAndDns(object source, RoutedEventArgs args)
    {
        // setup progress bar and console 
        _outputTextBoxNetwork.Text = "";
        NetworkAndDnsProgressBar.Opacity = 100;

        // run commands
        await ExecuteCommand("ipconfig /flushdns", _outputTextBoxNetwork, BorderNetworkAndDnsProgress, NetworkAndDnsProgress);
        NetworkAndDnsProgressBar.Value = 20;

        await ExecuteCommand("ipconfig /release", _outputTextBoxNetwork, BorderNetworkAndDnsProgress, NetworkAndDnsProgress);
        NetworkAndDnsProgressBar.Value = 40;

        await ExecuteCommand("ipconfig /renew", _outputTextBoxNetwork, BorderNetworkAndDnsProgress, NetworkAndDnsProgress);
        NetworkAndDnsProgressBar.Value = 60;

        await ExecuteCommand("ipconfig /registerdns", _outputTextBoxNetwork, BorderNetworkAndDnsProgress, NetworkAndDnsProgress);
        NetworkAndDnsProgressBar.Value = 80;

        await ExecuteCommand("netsh winsock reset", _outputTextBoxNetwork, BorderNetworkAndDnsProgress, NetworkAndDnsProgress);
        NetworkAndDnsProgressBar.Value = 100;

        // update UI to reflect all commands being run successfully
        BorderNetworkAndDnsProgress.Background = Brushes.Green;
        NetworkAndDnsProgress.Content = "Done!";
    }

    private async void RepairWindowsImage(object source, RoutedEventArgs args)
    {
        // setup progress bar and console 
        _outputTextBoxRepair.Text = "";
        RepairWindowsImageProgressBar.Opacity = 100;

        // run commands
        await ExecuteCommand("sfc /scannow", _outputTextBoxRepair, BorderWindowsImageRepairProgress, WindowsImageRepairProgress);
        RepairWindowsImageProgressBar.Value= 50;

        await ExecuteCommand("DISM /Online /Cleanup-Image /RestoreHealth", _outputTextBoxRepair, BorderWindowsImageRepairProgress, WindowsImageRepairProgress);
        RepairWindowsImageProgressBar.Value = 100;

        // update UI to reflect all commands being run successfully
        BorderWindowsImageRepairProgress.Background = Brushes.Green;
        WindowsImageRepairProgress.Content = "Done!";
    }

    // command to run CMD commands
    // written in cooperation with Bing Chat
    private async Task ExecuteCommand(string command, TextBox _outputTextBox, Border border, Label label)
    {
        try
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.Default, // set the desired encoding
                StandardErrorEncoding = Encoding.UTF8
            };
            process.StartInfo = startInfo;

            process.OutputDataReceived += async (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    // append the output to your TextBox
                    await Dispatcher.UIThread.InvokeAsync(() => _outputTextBox.Text += e.Data + Environment.NewLine);
                }
            };

            process.ErrorDataReceived += async (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    // Check if the error message contains the keyword "administrator"
                    if (e.Data.IndexOf("administrator", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            border.Background = Brushes.Red;
                            label.Content = "Error!";
                            _outputTextBox.Text += "You must run this program as an administrator.";
                        });
                    }
                    else
                    {
                        // Append other error output to your TextBox
                        await Dispatcher.UIThread.InvokeAsync(() => _outputTextBox.Text += "[ERROR] " + e.Data + Environment.NewLine);
                    }
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await process.WaitForExitAsync();

            // was the command successful?
            if (process.ExitCode == 0)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    _outputTextBox.Text += $"###########################################################\n" +
                                           $"\t\t\t\t\t\t\t\t\t\tProcess exited successfully!\n" +
                                           $"###########################################################\n\n";
                });
            }
            else
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    border.Background = Brushes.Red;
                    label.Content = "Error!";
                    _outputTextBox.Text += $"Error executing the command: '{command}'\n" +
                                           $"(Exit code: {process.ExitCode})\n\n";
                });
            }
        }
        catch (Exception ex)
        {
            await Dispatcher.UIThread.InvokeAsync(() => _outputTextBox.Text += $"An error occurred: {ex.Message}" + Environment.NewLine + "\n\n");
        }
    }
}
