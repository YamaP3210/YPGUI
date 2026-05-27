using MVF.Core;





namespace MVF;





public class MVFNode
{
    private Dictionary<Type , MVFComponent> _componentList = new Dictionary<Type , MVFComponent> ( );

    
    
    
    
    public object NodeHandle { get; private set; }




    
    
    public MVFNode ( object targetNodeHandle )
    {
        NodeHandle = targetNodeHandle;
    }



    public T AddComponent<T> ( ) where T : MVFComponent , new ( )
    {
        RemoveComponent<T> (  );
        var component = new T ( );
        component.BindNode ( this );
        _componentList.Add ( typeof ( T ) , component );
        return (T) _componentList [ typeof ( T ) ];
    }



    public void RemoveComponent<T> ( ) where T : MVFComponent
    {
        if ( _componentList.ContainsKey ( typeof ( T ) ) )
            _componentList.Remove ( typeof ( T ) );
    }



    public void RemoveComponent ( Type type )
    {
        if ( _componentList.ContainsKey ( type ) )
            _componentList.Remove ( type );
    }
    


    public T? GetComponent<T> ( ) where T : MVFComponent
    {
        if ( _componentList.ContainsKey ( typeof ( T ) ) )
            return ( T ) _componentList[ typeof ( T ) ];
        return null;
    }



    public void Dispose ( )
    {
        var _copied = new Dictionary<Type , MVFComponent> ( _componentList );
        foreach ( var component in _copied.Values )
            component.Dispose ( );
    }
}