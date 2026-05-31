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
        if (_targetEntity.GetWorldRect().Grow(SuccessDistanceThreshold).HasPoint(GetEntity().GlobalPosition))
        {
            SetState(UnitCommandState.Success);
            GetEntity().LinearVelocity = Vector2.Zero;
        }
        else
        {
            GetEntity().LinearVelocity = (_targetEntity.GlobalPosition - GetEntity().GlobalPosition).Normalized() * _moveToComponent.Speed;
        }
    }
}