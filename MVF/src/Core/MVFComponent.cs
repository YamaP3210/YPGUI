using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;





namespace MVF.Core;





public abstract class MVFComponent
{
    public MVFNode TargetNode { get; private set; } = null!;





    internal void BindNode ( MVFNode node )
    {
        TargetNode = node;
    }



    internal async Task InitializeAsync ( )
    {
        await OnInitializeAsync ( );
    }



    protected virtual Task OnInitializeAsync ( )
    {
        return Task.CompletedTask;
    }



    protected string[] GetComponentHtmlDirectoryPathList ( )
    {
        string componentNamespace = GetType ( ).Namespace ?? throw new InvalidOperationException ( "Component namespace was not found." );
        string namespaceMarker = $"{MVFCorePathDefines.UIDirectory}.View.";
        int namespaceMarkerIndex = componentNamespace.IndexOf ( namespaceMarker , StringComparison.Ordinal );

        if ( namespaceMarkerIndex < 0 )
            throw new InvalidOperationException ( $"Component namespace does not contain {namespaceMarker}: {componentNamespace}" );

        string relativeNamespace = componentNamespace.Substring ( namespaceMarkerIndex + namespaceMarker.Length );

        if ( relativeNamespace == string.Empty )
            return Array.Empty<string> ( );

        return relativeNamespace
            .Split ( '.' )
            .Where ( segment => segment != string.Empty )
            .ToArray ( );
    }



    protected async Task<string> ReadComponentHtmlAsync ( )
    {
        string componentHtmlFileName = $"{GetType ( ).Name}.html";
        string[] relativeDirectoryPathList = GetComponentHtmlDirectoryPathList ( );

        string componentHtmlFilePath = Path.Combine
        (
            AppDomain.CurrentDomain.BaseDirectory ,
            MVFCorePathDefines.UIRoot ,
            MVFCorePathDefines.UIDirectory ,
            MVFCorePathDefines.HtmlDirectory
        );

        foreach ( string relativeDirectoryPath in relativeDirectoryPathList )
            componentHtmlFilePath = Path.Combine ( componentHtmlFilePath , relativeDirectoryPath );

        componentHtmlFilePath = Path.Combine ( componentHtmlFilePath , componentHtmlFileName );

        return await File.ReadAllTextAsync ( componentHtmlFilePath );
    }



    protected async Task LoadComponentHtmlAsync ( )
    {
        string componentHtml = await ReadComponentHtmlAsync ( );
        await TargetNode.SetHtmlAsync ( componentHtml );
    }



    public void Dispose ( )
    {
        TargetNode?.RemoveComponent ( GetType ( ) );
    }
}
