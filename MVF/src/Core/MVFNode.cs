using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Web.WebView2.Wpf;

using MVF.Core;





namespace MVF;





public class MVFNode
{
    private static WebView2? s_viewHost;
    



    private Dictionary<Type , MVFComponent> _componentList = new Dictionary<Type , MVFComponent> ( );





    public object NodeHandle { get; private set; }



    public static WebView2 ViewHost
    {
        private get => s_viewHost ?? throw new InvalidOperationException ( "WebView is not attached." );
        set
        {
            if ( s_viewHost is not null )
                throw new InvalidOperationException ( "WebView was assigned already." );
            if ( value is null )
                throw new ArgumentNullException ( nameof ( value ) );
            s_viewHost = value;
        }
    }



    public static MVFNode Find ( string id )
    {
        return FindAsync ( id ).GetAwaiter ( ).GetResult ( );
    }



    public static async Task<MVFNode> FindAsync ( string id )
    {
        string idJson = JsonSerializer.Serialize ( id );
        string script = $"window.MVF.dom.findNodeHandle({idJson});";
        string result = await ViewHost.ExecuteScriptAsync ( script );
        int handle = JsonSerializer.Deserialize<int> ( result );

        return new MVFNode ( handle );
    }



    public MVFNode ( object targetNodeHandle )
    {
        NodeHandle = targetNodeHandle;
    }



    public T AddComponent<T> ( ) where T : MVFComponent , new ( )
    {
        RemoveComponent<T> (  );
        var component = new T ( );
        component.BindNode ( this );
        _componentList.Add ( typeof ( T ) , component );
        return (T) _componentList [ typeof ( T ) ];
    }



    public void RemoveComponent<T> ( ) where T : MVFComponent
    {
        if ( _componentList.ContainsKey ( typeof ( T ) ) )
            _componentList.Remove ( typeof ( T ) );
    }



    public void RemoveComponent ( Type type )
    {
        if ( _componentList.ContainsKey ( type ) )
            _componentList.Remove ( type );
    }



    public T? GetComponent<T> ( ) where T : MVFComponent
    {
        if ( _componentList.ContainsKey ( typeof ( T ) ) )
            return ( T ) _componentList[ typeof ( T ) ];
        return null;
    }



    public void Dispose ( )
    {
        var _copied = new Dictionary<Type , MVFComponent> ( _componentList );
        foreach ( var component in _copied.Values )
            component.Dispose ( );
    }



    public async Task SetHtmlAsync ( string viewHtml )
    {
        string handleJson = JsonSerializer.Serialize ( NodeHandle );
        string viewHtmlJson = JsonSerializer.Serialize ( viewHtml );
        string script = $"window.MVF.dom.setHtml({handleJson}, {viewHtmlJson});";
        await ViewHost.ExecuteScriptAsync ( script );
    }



    public async Task SetContextAsync<T> ( T context )
    {
        string handleJson = JsonSerializer.Serialize ( NodeHandle );
        string contextJson = JsonSerializer.Serialize ( context );
        string executionScript = $"window.MVF.dom.setContext({handleJson}, {contextJson});";

        await ViewHost.ExecuteScriptAsync ( executionScript );
    }



    public async Task<T?> GetContextAsync<T> ( )
    {
        string handleJson = JsonSerializer.Serialize ( NodeHandle );
        string executionScript = $"window.MVF.dom.getContext({handleJson});";
        string result = await ViewHost.ExecuteScriptAsync ( executionScript );

        return JsonSerializer.Deserialize<T> ( result );
    }
}
