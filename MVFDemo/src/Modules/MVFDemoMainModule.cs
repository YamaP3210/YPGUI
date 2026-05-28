using MVF.Core;

using MVF.Demo.UI.View.Widgets;





namespace MVF.Demo.Modules;





public class MVFDemoMainModule : MVFModule
{
    private MVFDemoMainWidget _mainWidget = null!;


    
    

    protected override async Task OnRequestLoadWidgetAsync ( )
    {
        _mainWidget = await LoadWidgetAsync<MVFDemoMainWidget> ( );
    }
}
