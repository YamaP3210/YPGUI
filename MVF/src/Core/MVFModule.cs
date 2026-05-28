using System.Threading.Tasks;





namespace MVF.Core;





public abstract class MVFModule
{
    public async Task InitializeAsync ( )
    {
        OnInitialize ( );
        await OnInitializeAsync ( );
        await OnRequestLoadWidgetAsync ( );
    }



    protected virtual void OnInitialize ( )
    {

    }



    protected virtual Task OnInitializeAsync ( )
    {
        return Task.CompletedTask;
    }



    protected abstract Task OnRequestLoadWidgetAsync ( );



    protected async Task<T> LoadWidgetAsync<T> ( ) where T : MVFWidget , new ( )
    {
        var widgetPlacementNode = await MVFNode.FindAsync ( MVFCoreNodeDefines.ViewCanvas );
        var widget = new T ( );
        var widgetNode = await widget.CreateWidgetNodeAsync ( widgetPlacementNode );

        return await widgetNode.AddComponentAsync ( widget );
    }
}
