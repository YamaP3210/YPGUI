namespace MVF.Core;





public abstract class MVFModule
{
    public async Task InitializeAsync ( )
    {
        OnInitialize ( );
        await  OnInitializeAsync ( );
    }



    protected virtual void OnInitialize ( )
    {
        
    }



    protected async Task OnInitializeAsync ( )
    {
        
    }
    


    protected abstract Task OnRequestLoadWidgetAsync ( );



    protected async Task<MVFNode> LoadWidgetAsync<T> ( ) where T : MVFComponent , new ( )
    {
        var type = typeof ( T );
        var widget = MVFNode.Find ( type.Name ).AddComponent<T> (  );
    }
}