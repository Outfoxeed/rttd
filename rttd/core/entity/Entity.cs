using Godot;

namespace RTTD;

[GlobalClass]
public partial class Entity : RigidBody2D, IEntityComponentsContainer
{
    public uint ID { get; } = Random.GetRandomUInt();
    [Export] private CollisionShape2D _entitySizeCollisionShape;
    private EntityComponentsContainer _componentsContainer;

    public override void _EnterTree()
    {
        base._EnterTree();

        string stringID = ID.ToString();
        string stringName = GetName().ToString();
        if (!stringName.EndsWith(stringID))
        {
            SetName(new StringName(stringName + stringID));
        }

        _componentsContainer ??= new EntityComponentsContainer(this);
    }

    public Rect2 GetWorldRect()
    {
        switch (_entitySizeCollisionShape.Shape)
        {
            case RectangleShape2D rectangleShape2D:
                return new Rect2(GlobalPosition - rectangleShape2D.Size * 0.5f, rectangleShape2D.Size);
            case CircleShape2D circleShape2D:
                return new Rect2(GlobalPosition - Vector2.One * circleShape2D.Radius, Vector2.One * circleShape2D.Radius * 2);
        }

        return new Rect2(GlobalPosition, Vector2.Zero).Grow(16);
    }

    public void AddComponent<T>(T component) where T : IEntityComponent => _componentsContainer.AddComponent(component);
    public void RemoveComponent<T>(T component) where T : IEntityComponent => _componentsContainer.RemoveComponent(component);
    public void RemoveAllComponents<T>() where T : IEntityComponent => _componentsContainer.RemoveAllComponents<T>();
    public bool HasComponent<T>() where T : IEntityComponent => _componentsContainer.HasComponent<T>();
    public bool TryGetComponent<T>(out T component) where T : IEntityComponent => _componentsContainer.TryGetComponent(out component);
    public T GetComponent<T>() where T : IEntityComponent => _componentsContainer.GetComponent<T>();
    public bool TryGetAllComponents<T>(out T[] components) where T : IEntityComponent => _componentsContainer.TryGetAllComponents(out components);
    public T[] GetAllComponents<T>() where T : IEntityComponent => _componentsContainer.GetAllComponents<T>();
    public IEntityComponent[] GetAllComponents() => _componentsContainer.GetAllComponents();
} 