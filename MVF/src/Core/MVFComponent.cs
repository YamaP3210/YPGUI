using System.IO;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;





namespace MVF.Core;





public abstract class MVFComponent
{
    public MVFNode TargetNode { get; private set; } = null!;
    
    
    
    
    
    internal void BindNode ( MVFNode node )
    {
        TargetNode = node;
    }



    public void Dispose ( )
    {
        TargetNode?.RemoveComponent ( GetType ( ) );
    }
}