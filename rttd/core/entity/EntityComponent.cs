using Godot;

namespace RTTD;

[GlobalClass]
public partial class EntityComponent : Node2D
{
    private Entity _entityOwner;

    public EntityComponent()
    {
        SetName(GetType().Name);
    }

    public Entity GetEntityOwner()
    {
        if (_entityOwner == null)
            _entityOwner = GetParent<Entity>();
        return _entityOwner;
    }
}