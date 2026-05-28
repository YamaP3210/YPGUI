using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Web.WebView2.Wpf;

using MVF.Core;





namespace MVF;





public class MVFController
{
    private const string ViewCanvasHtmlResourceName = "MVF.UI.View.Canvas.MVFViewCanvas.html";
    
    
    
    private const string ViewCanvasCssResourceName = "MVF.UI.View.Canvas.MVFViewCanvas.css";



    private const string ViewCanvasDomJsResourceName = "MVF.UI.JS.MVFViewCanvasDom.js";
    
    
    
    
    
    private static MVFController? s_instance;

    
    

    private Panel _container;



    private MVFNode _viewCanvasNode;
    
    
    
    private WebView2 _viewHost;
    
    
    
    
    
    public static async Task RunAsync ( Panel mvfContainer )
    {
        s_instance = new MVFController (  ) ;
        await s_instance.Initialize ( mvfContainer );
        return;
    }
    


    private MVFController ( )
    {

    }



    public async Task Initialize ( Panel container )
    {        
        _container = container;
        
        var mvfRootElement = LoadFrameElement( "MVFRoot" );
        
        var mvfClientAreaElement = LoadFrameElement( "MVFClientArea" );
        var mvfClientAreaHostElement = FindFrameElement<ContentControl> ( mvfRootElement , "MVFClientAreaHost" );
        mvfClientAreaHostElement.Content = mvfClientAreaElement;
        _container.Children.Add ( mvfRootElement );
        
        _viewHost = FindFrameElement<WebView2> ( mvfClientAreaElement , "MVFViewHost" );
        MVFNode.ViewHost = _viewHost;
        
        var viewCanvasHtml = await ReadResourceTextAsync ( ViewCanvasHtmlResourceName );
        var viewCanvasCss = await ReadResourceTextAsync ( ViewCanvasCssResourceName );
        var viewCanvasDomJs = await ReadResourceTextAsync ( ViewCanvasDomJsResourceName );
        var composedViewCanvasHtml = viewCanvasHtml.Replace ( "{{MVFViewCanvasStyle}}", viewCanvasCss, StringComparison.Ordinal );
        composedViewCanvasHtml = composedViewCanvasHtml.Replace ( "{{MVFViewCanvasDomScript}}", $"<script>{viewCanvasDomJs}</script>", StringComparison.Ordinal );
        _viewHost.NavigateToString (  composedViewCanvasHtml );
        _viewCanvasNode = await MVFNode.FindAsync ("MVFViewCanvas" );
    }
    
    
    
    public T FindFrameElement<T> ( FrameworkElement rootElement , string elementName ) where T : FrameworkElement
    {
        object? element = rootElement.FindName ( elementName );
        if ( element is T typedElement )
            return typedElement;
        throw new InvalidOperationException ( $"Frame element was not found: {elementName}" );
    }
    
    
    
    private FrameworkElement LoadFrameElement ( string frameFileName )
    {
        Uri frameUri = new ( $"/MVF;component/src/UI/Frame/{frameFileName}", UriKind.Relative );

        return ( FrameworkElement ) Application.LoadComponent ( frameUri );
    }
    


    private async Task<string> ReadResourceTextAsync ( string resourceName )
    {
        Assembly assembly = typeof ( MVFComponent ).Assembly;
        await using Stream? resourceStream = assembly.GetManifestResourceStream ( resourceName );

        if ( resourceStream is null )
        {
            throw new InvalidOperationException ( $"MVF resource was not found: {resourceName}" );
        }

        using StreamReader resourceReader = new ( resourceStream );

        return await resourceReader.ReadToEndAsync ( );
    }



    public async Task<T> LoadModuleAsync<T> ( ) where T : MVFModule , new ( )
    {
        var module = new T ( );
        await module.InitializeAsync ( );
        return module;
    }
    
    
    
    public async Task LoadStyleAsync ( Uri styleUri )
    {
        string styleUriJson = JsonSerializer.Serialize ( styleUri.ToString ( ) );
        string script = $"window.MVF.dom.loadStyle({styleUriJson});";
        await _viewHost.ExecuteScriptAsync ( script );
    }



    public async Task LoadJSAsync ( Uri jsUri )
    {
        string jsUriJson = JsonSerializer.Serialize ( jsUri.ToString ( ) );
        string script = $"window.MVF.dom.loadJS({jsUriJson});";
        await _viewHost.ExecuteScriptAsync ( script );
    }
}
