using Microsoft.UI.Xaml.Controls;

namespace Windows_Diagnostic_Tools_rewrite.Views;

public sealed partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        ContentFrame.Navigate(typeof(Home));
    }

    private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer is NavigationViewItem { Tag: string tag })
        {
            switch (tag)
            {
                case "Home":
                    ContentFrame.Navigate(typeof(Home));
                    break;
                case "WindowsRepair":
                    ContentFrame.Navigate(typeof(WindowsRepair));
                    break;
                case "NetworkRepair":
                    ContentFrame.Navigate(typeof(NetworkRepair));
                    break;
            }
        }
    }
}