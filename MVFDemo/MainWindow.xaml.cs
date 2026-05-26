using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Web.WebView2.Wpf;

using MVF.UI.Modules;





namespace MVFDemo;





public partial class MainWindow : Window
{
    private readonly MVFRootModule _rootModule = new ( );





    public MainWindow ( )
    {
        InitializeComponent ( );
    }



    protected override async void OnContentRendered ( EventArgs eventArgs )
    {
        base.OnContentRendered ( eventArgs );

        await BuildMVFDemoAsync ( );
    }



    private async Task BuildMVFDemoAsync ( )
    {
        FrameworkElement rootFrame = LoadMVFFrameElement ( "MVFRoot.xaml" );
        FrameworkElement clientAreaFrame = LoadMVFFrameElement ( "MVFClientArea.xaml" );

        ContentControl clientAreaHost = FindFrameElement<ContentControl> ( rootFrame, "MVFClientAreaHost" );
        clientAreaHost.Content = clientAreaFrame;

        WebView2 viewHost = FindFrameElement<WebView2> ( clientAreaFrame, "MVFViewHost" );

        MVFWindowRoot.Children.Add ( rootFrame );

        await _rootModule.AttachViewHostAsync ( viewHost );
        await _rootModule.LoadDefaultFrameViewAsync ( );
        await _rootModule.SetViewHtmlAsync ( "MVFModuleRoot", await ReadViewTextAsync ( "Widgets", "MVFDemoMainWidget.html" ) );
        await _rootModule.SetViewHtmlAsync ( "MVFDemoMainComponentArea", await ReadViewTextAsync ( "Components", "MVFDemoMainWidget", "MVFDemoMainPanel.html" ) );
        await _rootModule.SetViewHtmlAsync ( "MVFDemoMainContent", await ReadViewTextAsync ( "Parts", "MVFDemoStatusPanel.html" ) );
    }



    private static FrameworkElement LoadMVFFrameElement ( string frameFileName )
    {
        Uri frameUri = new ( $"/MVF;component/src/UI/Frame/{frameFileName}", UriKind.Relative );

        return ( FrameworkElement ) Application.LoadComponent ( frameUri );
    }



    private static T FindFrameElement<T> ( FrameworkElement rootElement, string elementName ) where T : FrameworkElement
    {
        object? element = rootElement.FindName ( elementName );

        if ( element is T typedElement )
        {
            return typedElement;
        }

        throw new InvalidOperationException ( $"Frame element was not found: {elementName}" );
    }



    private static Task<string> ReadViewTextAsync ( params string[] pathParts )
    {
        return File.ReadAllTextAsync ( GetViewPath ( pathParts ) );
    }



    private static string GetViewPath ( params string[] pathParts )
    {
        string[] fullPathParts = new string[pathParts.Length + 4];
        fullPathParts[0] = AppContext.BaseDirectory;
        fullPathParts[1] = "src";
        fullPathParts[2] = "UI";
        fullPathParts[3] = "View";

        Array.Copy ( pathParts, 0, fullPathParts, 4, pathParts.Length );

        return Path.GetFullPath ( Path.Combine ( fullPathParts ) );
    }
}
