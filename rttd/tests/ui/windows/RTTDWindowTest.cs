using Godot;

namespace RTTD.Tests;

[GlobalClass]
public partial class RTTDWindowTest : RTTDWindowEntityComponent
{
    [Export] private BaseButton _button;

    [Export] private ResourceType _resourceType;
    [Export] private int _resourceAmount;
    [Export] private PackedScene _entityPackedScene;

    public override void _EnterTree()
    {
        base._EnterTree();
        if(_button is not null) _button.Pressed += OnButtonPressed;
    }

    public override void _ExitTree()
    {
        if(_button is not null) _button.Pressed -= OnButtonPressed;
        base._ExitTree();
    }

    private void OnButtonPressed()
    {
        if (!PlayerResourcesManager.Instance.Resources.TryBuy(_resourceType, _resourceAmount))
            return;
        
        Entity entity = _entityPackedScene.Instantiate<Entity>();
        if (entity is null)
            return;

        Entity ownerEntity = WindowOwner.GetEntity();
        ownerEntity.GetParent().AddChild(entity);
        entity.SetGlobalPosition(ownerEntity.GetGlobalPosition());
        if (entity.TryGetComponent(out UnitComponent unitComponent))
        {
            unitComponent.AddCommand(new MoveToPositionCommand(ownerEntity.GetGlobalPosition() + Random.GetRandomPointAlongCircle(64)));
        }
    }
}