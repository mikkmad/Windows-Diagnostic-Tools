using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Security.Principal;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Windows_Diagnostic_Tools_rewrite.Views;

public sealed partial class Home : Page
{
    public Home()
    {
        InitializeComponent();
        
        // Check if running as admin
        bool isAdmin = IsRunningAsAdmin();
        AdminWarningInfoBar.Visibility = isAdmin ? Visibility.Collapsed : Visibility.Visible;
        
        // Set GitHub logo based on theme
        SetGitHubLogo();
    }

    private static bool IsRunningAsAdmin()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    private void SetGitHubLogo()
    {
        var isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
        var imagePath = isDark 
            ? "ms-appx:///Assets/github-mark-white.png"
            : "ms-appx:///Assets/github-mark.png";
        
        GitHubLogo.Source = new BitmapImage(new System.Uri(imagePath));
    }
}