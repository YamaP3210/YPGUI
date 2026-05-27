using System.Windows;
using MVF.Core;


namespace MVF;





public static class MVFExtensions
{
    public static T FindElement<T> ( this FrameworkElement self , string elementName ) where T : FrameworkElement
    {
        object? element = self.FindName ( elementName );

        if ( element is T typedElement )
        {
            return typedElement;
        }

        throw new InvalidOperationException ( $"Frame element was not found: {elementName}" );
    }



    public static T AttachComponent<T> ( this FrameworkElement self , T element ) where T : MVFComponent , new ( )
    {
        var component = new T ( );
        component.AttachElement ( self );
        return component;
    }
}