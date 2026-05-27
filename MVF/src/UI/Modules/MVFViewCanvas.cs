using System.Text.Json;

using MVF.Core;




namespace MVF.Modules;





public class MVFViewCanvas : MVFComponent
{
    protected async Task LoadStyleAsync ( Uri styleUri )
    {
        if ( ViewHost is null )
            throw new InvalidOperationException ( "ViewHost is not attached." );

        string styleUriJson = JsonSerializer.Serialize ( styleUri.ToString ( ) );
        string script = $"window.MVF.dom.loadStyle({styleUriJson});";

        await ViewHost.ExecuteScriptAsync ( script );
    }
}