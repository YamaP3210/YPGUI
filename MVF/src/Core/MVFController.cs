using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

using MVF.Core;





namespace MVF;





public class MVFController
{
    private const string ViewCanvasHtmlResourceName = "MVF.UI.View.Canvas.MVFViewCanvas.html";



    private const string ViewCanvasCssResourceName = "MVF.UI.View.Canvas.MVFViewCanvas.css";



    private const string ViewCanvasDomJsResourceName = "MVF.UI.JS.MVFViewCanvasDom.js";





    private static MVFController? s_instance;



    private Panel _container = null!;



    private ContentControl _menuBarHost = null!;



    private MVFWindowConfigurator _windowConfigurator = null!;



    private MVFNode _viewCanvasNode = null!;



    private WebView2 _viewHost = null!;



    public static async Task<MVFController> RunAsync ( Panel mvfContainer )
    {
        if ( s_instance is not null )
            throw new InvalidOperationException ( "MVF is running already." );

        s_instance = new MVFController ( );
        await s_instance.InitializeAsync ( mvfContainer );
        return s_instance;
    }



    private MVFController ( )
    {

    }



    public MVFWindowConfigurator GetWindowConfigurator ( )
    {
        return _windowConfigurator;
    }



    public async Task<T> LoadModuleAsync<T> ( ) where T : MVFModule , new ( )
    {
        var module = new T ( );
        await module.InitializeAsync ( );
        return module;
    }



    public async Task LoadStyleAsync ( Uri styleUri )
    {
        var styleUriJson = JsonSerializer.Serialize ( styleUri.ToString ( ) );
        var script = $"window.MVF.dom.loadStyle({styleUriJson});";
        await _viewHost.ExecuteScriptAsync ( script );
    }



    public async Task LoadJSAsync ( Uri jsUri )
    {
        var jsUriJson = JsonSerializer.Serialize ( jsUri.ToString ( ) );
        var script = $"window.MVF.dom.loadJS({jsUriJson});";
        await _viewHost.ExecuteScriptAsync ( script );
    }



    private async Task InitializeAsync ( Panel container )
    {
        _container = container;

        var mvfRootElement = LoadFrameElement ( "MVFRoot" );
        _menuBarHost = FindFrameElement<ContentControl> ( mvfRootElement , "MVFMenuBarHost" );

        var menuBarElement = LoadFrameElement ( "MVFMenuBar" );
        var menuRoot = FindFrameElement<Menu> ( menuBarElement , "MVFMenuRoot" );
        _menuBarHost.Content = menuBarElement;

        var targetWindow = Window.GetWindow ( _container ) ?? throw new InvalidOperationException ( "MVF window was not found." );
        _windowConfigurator = new MVFWindowConfigurator ( targetWindow , menuRoot );

        var mvfClientAreaElement = LoadFrameElement ( "MVFClientArea" );
        var mvfClientAreaHostElement = FindFrameElement<ContentControl> ( mvfRootElement , "MVFClientAreaHost" );
        mvfClientAreaHostElement.Content = mvfClientAreaElement;
        _container.Children.Add ( mvfRootElement );

        var viewCanvasHtml = await ReadResourceTextAsync ( ViewCanvasHtmlResourceName );
        var viewCanvasCss = await ReadResourceTextAsync ( ViewCanvasCssResourceName );
        var viewCanvasDomJs = await ReadResourceTextAsync ( ViewCanvasDomJsResourceName );
        var composedViewCanvasHtml = viewCanvasHtml.Replace ( "{{MVFViewCanvasStyle}}" , viewCanvasCss , StringComparison.Ordinal );
        composedViewCanvasHtml = composedViewCanvasHtml.Replace ( "{{MVFViewCanvasDomScript}}" , $"<script>{viewCanvasDomJs}</script>" , StringComparison.Ordinal );

        _viewHost = FindFrameElement<WebView2> ( mvfClientAreaElement , "MVFViewHost" );
        MVFNode.AttachViewHost ( _viewHost );
        await _viewHost.EnsureCoreWebView2Async ( );

        var navigationTask = WaitForNavigationCompletedAsync ( _viewHost );
        _viewHost.NavigateToString ( composedViewCanvasHtml );
        await navigationTask;

        _viewCanvasNode = await MVFNode.FindAsync ( MVFCoreNodeDefines.ViewCanvas );
    }



    private Task WaitForNavigationCompletedAsync ( WebView2 viewHost )
    {
        var taskCompletionSource = new TaskCompletionSource ( );

        void OnNavigationCompleted ( object? sender , CoreWebView2NavigationCompletedEventArgs eventArgs )
        {
            viewHost.NavigationCompleted -= OnNavigationCompleted;

            if ( eventArgs.IsSuccess )
                taskCompletionSource.SetResult ( );
            else
                taskCompletionSource.SetException
                (
                    new InvalidOperationException ( "WebView navigation failed." )
                );
        }

        viewHost.NavigationCompleted += OnNavigationCompleted;

        return taskCompletionSource.Task;
    }



    private T FindFrameElement<T> ( FrameworkElement rootElement , string elementName ) where T : FrameworkElement
    {
        object? element = rootElement.FindName ( elementName );

        if ( element is T typedElement )
            return typedElement;

        throw new InvalidOperationException ( $"Frame element was not found: {elementName}" );
    }



    private FrameworkElement LoadFrameElement ( string frameFileName )
    {
        var frameUri = new Uri ( $"/MVF;component/src/UI/Frame/{frameFileName}.xaml" , UriKind.Relative );

        return ( FrameworkElement ) Application.LoadComponent ( frameUri );
    }



    private async Task<string> ReadResourceTextAsync ( string resourceName )
    {
        var assembly = typeof ( MVFComponent ).Assembly;
        await using Stream? resourceStream = assembly.GetManifestResourceStream ( resourceName );

        if ( resourceStream is null )
            throw new InvalidOperationException ( $"MVF resource was not found: {resourceName}" );

        using var resourceReader = new StreamReader ( resourceStream );

        return await resourceReader.ReadToEndAsync ( );
    }
}
