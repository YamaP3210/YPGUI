using System;
using System.Windows;
using System.Windows.Controls;





namespace MVF.Core;





public class MVFWindowConfigurator
{
    private Window _targetWindow;



    private Menu _menuRoot;



    internal MVFWindowConfigurator ( Window targetWindow , Menu menuRoot )
    {
        _targetWindow = targetWindow;
        _menuRoot = menuRoot;
    }



    public double Width
    {
        get => _targetWindow.Width;
        set => _targetWindow.Width = value;
    }



    public double Height
    {
        get => _targetWindow.Height;
        set => _targetWindow.Height = value;
    }



    public bool IsFullScreen
    {
        get => _targetWindow.WindowState == WindowState.Maximized && _targetWindow.WindowStyle == WindowStyle.None;
        set
        {
            if ( value )
            {
                _targetWindow.WindowStyle = WindowStyle.None;
                _targetWindow.WindowState = WindowState.Maximized;
                return;
            }

            _targetWindow.WindowStyle = HasWindowControl ? WindowStyle.SingleBorderWindow : WindowStyle.None;
            _targetWindow.WindowState = WindowState.Normal;
        }
    }



    public bool HasWindowControl
    {
        get => _targetWindow.WindowStyle != WindowStyle.None;
        set => _targetWindow.WindowStyle = value ? WindowStyle.SingleBorderWindow : WindowStyle.None;
    }



    public bool CanResize
    {
        get => _targetWindow.ResizeMode == ResizeMode.CanResize || _targetWindow.ResizeMode == ResizeMode.CanResizeWithGrip;
        set => _targetWindow.ResizeMode = value ? ResizeMode.CanResize : ResizeMode.NoResize;
    }



    public void SetMenu ( string categoryName , string itemName , Action action )
    {
        var categoryMenuItem = GetOrCreateMenuItem ( _menuRoot.Items , categoryName );
        var targetMenuItem = GetOrCreateMenuItem ( categoryMenuItem.Items , itemName );

        targetMenuItem.Click += ( sender , eventArgs ) => action ( );
    }



    public void SetMenu ( string categoryName , string itemName , string[] choiceNameList , Action[] actionList )
    {
        if ( choiceNameList.Length != actionList.Length )
            throw new ArgumentException ( "Choice and action counts must match." );

        var categoryMenuItem = GetOrCreateMenuItem ( _menuRoot.Items , categoryName );
        var parentItemMenuItem = GetOrCreateMenuItem ( categoryMenuItem.Items , itemName );

        parentItemMenuItem.Items.Clear ( );

        for ( int choiceIndex = 0 ; choiceIndex < choiceNameList.Length ; choiceIndex++ )
        {
            var choiceMenuItem = new MenuItem
            {
                Header = choiceNameList [ choiceIndex ]
            };

            var action = actionList [ choiceIndex ];
            choiceMenuItem.Click += ( sender , eventArgs ) => action ( );
            parentItemMenuItem.Items.Add ( choiceMenuItem );
        }
    }



    private MenuItem GetOrCreateMenuItem ( ItemCollection itemList , string headerName )
    {
        foreach ( var item in itemList )
        {
            if ( item is MenuItem menuItem && Equals ( menuItem.Header , headerName ) )
                return menuItem;
        }

        var createdMenuItem = new MenuItem
        {
            Header = headerName
        };

        itemList.Add ( createdMenuItem );
        return createdMenuItem;
    }
}
