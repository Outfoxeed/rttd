using Godot;

namespace RTTD;

[GlobalClass]
public partial class WindowOpenerComponent : EntityComponent
{
    [Export] private PackedScene _windowPackedScene;
    
    public override void _EnterTree()
    {
        base._EnterTree();
        GetEntity().InputEvent += OnEntityInputEvent;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        GetEntity().InputEvent -= OnEntityInputEvent;
    }
    
    private void OnEntityInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton { ButtonIndex:MouseButton.Left, Pressed: true })
        {
            UIManager.Instance.AddWindow(this, _windowPackedScene, new Vector2(100, 100));
            GetViewport().SetInputAsHandled();
        }
    }
}