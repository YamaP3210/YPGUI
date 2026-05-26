using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;





namespace MVF.UI.Modules;





/// <summary>
/// Acts as the ViewModel for the XAML-based MVF root control.
/// </summary>
public class MVFRootModule
{
    private const string ViewCanvasHtmlResourceName = "MVF.UI.View.Canvas.MVFViewCanvas.html";



    private const string ViewCanvasCssResourceName = "MVF.UI.View.Canvas.MVFViewCanvas.css";





    private WebView2? _viewHost;





    public WebView2? ViewHost
    {
        get
        {
            return _viewHost;
        }
    }





    public async Task AttachViewHostAsync ( WebView2 viewHost )
    {
        ArgumentNullException.ThrowIfNull ( viewHost );

        _viewHost = viewHost;

        await _viewHost.EnsureCoreWebView2Async ( );
    }



    public async Task LoadFrameViewAsync ( Uri frameViewUri )
    {
        if ( _viewHost is null )
        {
            throw new InvalidOperationException ( "MVFViewHost is not attached." );
        }

        TaskCompletionSource frameViewLoaded = new ( );

        void OnNavigationCompleted ( object? sender, CoreWebView2NavigationCompletedEventArgs eventArgs )
        {
            _viewHost.NavigationCompleted -= OnNavigationCompleted;

            if ( eventArgs.IsSuccess )
            {
                frameViewLoaded.SetResult ( );
                return;
            }

            frameViewLoaded.SetException ( new InvalidOperationException ( $"Frame view navigation failed: {eventArgs.WebErrorStatus}" ) );
        }

        _viewHost.NavigationCompleted += OnNavigationCompleted;
        _viewHost.Source = frameViewUri;

        await frameViewLoaded.Task;
    }



    public async Task LoadDefaultFrameViewAsync ( )
    {
        if ( _viewHost is null )
        {
            throw new InvalidOperationException ( "MVFViewHost is not attached." );
        }

        string viewCanvasHtml = await ReadResourceTextAsync ( ViewCanvasHtmlResourceName );
        string viewCanvasCss = await ReadResourceTextAsync ( ViewCanvasCssResourceName );
        string composedViewCanvasHtml = viewCanvasHtml.Replace ( "{{MVFViewCanvasStyle}}", viewCanvasCss, StringComparison.Ordinal );

        _viewHost.NavigateToString ( composedViewCanvasHtml );
    }



    public async Task LoadStyleAsync ( Uri styleUri )
    {
        if ( _viewHost is null )
        {
            throw new InvalidOperationException ( "MVFViewHost is not attached." );
        }

        string styleUriJson = JsonSerializer.Serialize ( styleUri.ToString ( ) );
        string script = $"window.MVF.dom.loadStyle({styleUriJson});";

        await _viewHost.ExecuteScriptAsync ( script );
    }



    public async Task SetViewHtmlAsync ( string targetUiId, string viewHtml )
    {
        if ( _viewHost is null )
        {
            throw new InvalidOperationException ( "MVFViewHost is not attached." );
        }

        string targetUiIdJson = JsonSerializer.Serialize ( targetUiId );
        string viewHtmlJson = JsonSerializer.Serialize ( viewHtml );
        string script = $"window.MVF.dom.setHtml({targetUiIdJson}, {viewHtmlJson});";

        await _viewHost.ExecuteScriptAsync ( script );
    }



    private static async Task<string> ReadResourceTextAsync ( string resourceName )
    {
        Assembly assembly = typeof ( MVFRootModule ).Assembly;
        await using Stream? resourceStream = assembly.GetManifestResourceStream ( resourceName );

        if ( resourceStream is null )
        {
            throw new InvalidOperationException ( $"MVF resource was not found: {resourceName}" );
        }

        using StreamReader resourceReader = new ( resourceStream );

        return await resourceReader.ReadToEndAsync ( );
    }
}
