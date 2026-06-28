using Godot;

namespace RTTD;

[GlobalClass]
public partial class EntityComponent : Node2D, IEntityComponent
{
    public bool Enabled { get; set; } = true;
    
    private Entity _entity;

    public Entity GetEntity()
    {
        if (_entity == null)
            _entity = GetParent<Entity>();
        return _entity;
    }
}