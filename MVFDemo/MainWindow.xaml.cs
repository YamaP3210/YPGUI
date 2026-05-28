using System.Windows;

using MVF.Demo.Modules;


namespace MVF.Demo;





public partial class MainWindow : Window
{
    public MainWindow ( )
    {
        InitializeComponent ( );
    }



    protected override async void OnContentRendered ( EventArgs eventArgs )
    {
        base.OnContentRendered ( eventArgs );

        var mvfController = await MVFController.RunAsync ( MVFContainer );
        var windowConfigurator = mvfController.GetWindowConfigurator ( );

        await mvfController.LoadModuleAsync<MVFDemoMainModule> ( );

        windowConfigurator.SetMenu ( "ファイル" , "閉じる" , ( ) => Close ( ) );
    }
}
