using Godot;

namespace RTTD;

[GlobalClass]
public partial class Entity : RigidBody2D
{
    public uint ID { get; } = Random.GetRandomUInt();
    [Export] private CollisionShape2D _entitySizeCollisionShape;

    public override void _EnterTree()
    {
        base._EnterTree();

        string stringID = ID.ToString();
        string stringName = GetName().ToString();
        if (!stringName.EndsWith(stringID))
        {
            SetName(new StringName(stringName + stringID));
        }
    }

    public bool TryGetComponent<T>(out T component) where T : EntityComponent
    {
        component = GetComponent<T>();
        return component != null;
    }

    public T GetComponent<T>() where T : EntityComponent
    {
        T result = GetNode<T>(typeof(T).Name);
        if (result == null)
        {
            Logger.LogError(this, $"No component '{typeof(T).Name}' on entity '{GetName()}'");
        }
        return GetNode<T>(typeof(T).Name);
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
} 