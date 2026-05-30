using System;
using Godot;

namespace RTTD;

public class MoveToEntityCommand : MoveToCommandBase
{
    private readonly Entity _targetEntity;

    public MoveToEntityCommand(Entity targetEntity, Action onSuccess = null, Action onFailed = null) : base(onSuccess, onFailed)
    {
        _targetEntity = targetEntity;
    }

    protected override void UpdateVelocity()
    {
        if (_targetEntity.GetWorldRect().Grow(SuccessDistanceThreshold).HasPoint(GetUnit().GetEntityOwner().GlobalPosition))
        {
            SetState(UnitCommandState.Success);
            GetUnit().GetEntityOwner().LinearVelocity = Vector2.Zero;
        }
        else
        {
            GetUnit().GetEntityOwner().LinearVelocity = (_targetEntity.GlobalPosition - GetUnit().GetEntityOwner().GlobalPosition).Normalized() * _moveToComponent.Speed;
        }
    }
}