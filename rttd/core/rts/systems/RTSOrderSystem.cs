using System.Collections.Generic;
using Godot;

namespace RTTD;

[GlobalClass]
public partial class RTSOrderSystem : Node2D
{
    [Export] private RTSSelectionSystem _selectionSystem;
    [Export] private ShapeCast2D _orderShapeCast;

    private readonly List<Entity> _entitiesCache = new();
    
    public override void _EnterTree()
    {
        base._EnterTree();
        _orderShapeCast.Enabled = false;
        _orderShapeCast.TargetPosition = Vector2.Zero;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (_selectionSystem.Selected.Count == 0)
            return;
        
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Right })
        {
            Vector2 globalMousePosition = GetGlobalMousePosition();
            OrderMode orderMode = Input.IsKeyPressed(Key.Shift) ? OrderMode.Additive : OrderMode.Single; 
            ApplyOrder(_selectionSystem.Selected, globalMousePosition, GetClosestEntity(globalMousePosition), orderMode);

            GetViewport().SetInputAsHandled();
        }
    }
    
    private Entity GetClosestEntity(Vector2 worldPosition)
    {
        _orderShapeCast.GlobalPosition = worldPosition;
        _orderShapeCast.ForceShapecastUpdate();
            
        _entitiesCache.Clear();
        foreach (Variant variant in _orderShapeCast.CollisionResult)
        {
            if (variant.AsGodotDictionary()["collider"].As<Entity>() is { } entity)
            {
                _entitiesCache.Add(entity);
            }
        }

        if (_entitiesCache.Count == 0)
            return null;

        float closestSquaredistance = float.MaxValue;
        Entity closestEntity = null;
        for (var i = 0; i < _entitiesCache.Count; i++)
        {
            Entity entity = _entitiesCache[i];
            float squaredDistance = worldPosition.DistanceSquaredTo(entity.GlobalPosition);
            if (squaredDistance < closestSquaredistance)
            {
                closestSquaredistance = squaredDistance;
                closestEntity = entity;
            }
        }
        return closestEntity;
    }
}