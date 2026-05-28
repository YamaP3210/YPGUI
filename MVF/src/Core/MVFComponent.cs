namespace MVF.Core;





public abstract class MVFComponent
{
    public MVFNode TargetNode { get; private set; } = null!;





    public MVFComponent ( )
    {
        
    }
    
    
    
    internal void BindNode ( MVFNode node )
    {
        TargetNode = node;
    }



    public void Dispose ( )
    {
        TargetNode?.RemoveComponent ( GetType ( ) );
    }
}