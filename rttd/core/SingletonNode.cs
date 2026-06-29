using Godot;

namespace RTTD;

public abstract partial class SingletonNode<T> : Node where T : SingletonNode<T>
{
    public static T Instance { get; private set; }
    
    public override void _EnterTree()
    {
        base._EnterTree();
        
        if (Instance.IsValid() && Instance != this)
        {
            QueueFree();
            return;
        }

        Instance = this as T;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (Instance != this) 
            return;
        Instance = null;
    }
}