using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Web.WebView2.Wpf;

using MVF.Core;
using MVF.Modules;





namespace MVF;





public class MVFController
{
    private static MVFController? s_instance;

    
    
    private const string ViewCanvasHtmlResourceName = "MVF.UI.View.Canvas.MVFViewCanvas.html";
    
    
    
    private const string ViewCanvasCssResourceName = "MVF.UI.View.Canvas.MVFViewCanvas.css";
    


    private Panel _container;
    
    
    
    private WebView2 _viewHost;



    private MVFViewCanvas _viewCanvas;
    
    
    
    
    
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
        
        var mvfRootFrame = LoadFrameElement( "MVFRootFrame" );
        
        var mvfClientAreaFrame = LoadFrameElement( "MVFClientAreaFrame" );
        var mvfClientAreaHost = mvfRootFrame.FindElement<ContentControl> ( "MVFClientAreaHost" );
        mvfClientAreaHost.Content = mvfClientAreaFrame;
        _container.Children.Add ( mvfRootFrame );
        
        _viewHost = mvfClientAreaFrame.FindElement<WebView2> ( "MVFViewHost" );
        await MVFComponent.AttachViewHostAsync ( _viewHost );
        
        var viewCanvasHtml = await ReadResourceTextAsync ( ViewCanvasHtmlResourceName );
        var viewCanvasCss = await ReadResourceTextAsync ( ViewCanvasCssResourceName );
        var composedViewCanvasHtml = viewCanvasHtml.Replace ( "{{MVFViewCanvasStyle}}", viewCanvasCss, StringComparison.Ordinal );
        _viewHost.NavigateToString (  composedViewCanvasHtml );

        
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
}