using System.Windows;

using MVF;






namespace MVFDemo;





public partial class MainWindow : Window
{
    public MainWindow ( )
    {
        InitializeComponent ( );
    }



    protected override async void OnContentRendered ( EventArgs eventArgs )
    {
        base.OnContentRendered ( eventArgs );

        await MVFController.Start ( MVFContainer );
    }
}
