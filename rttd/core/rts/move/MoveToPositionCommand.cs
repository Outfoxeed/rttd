using System;
using Godot;

namespace RTTD;

public class MoveToPositionCommand : MoveToCommandBase
{
    private readonly Vector2 _targetPosition;
    
    public MoveToPositionCommand(Vector2 targetPosition, Action onSuccess = null, Action onFailed = null) : base(onSuccess, onFailed)
    {
        _targetPosition = targetPosition;
    }

    protected override void UpdateVelocity()
    {
        if (_targetPosition.DistanceSquaredTo(GetUnit().GetEntityOwner().GlobalPosition) <= SuccessDistanceThreshold * SuccessDistanceThreshold)
        {
            SetState(UnitCommandState.Success);
            GetUnit().GetEntityOwner().LinearVelocity = Vector2.Zero;
        }
        else
        {
            GetUnit().GetEntityOwner().LinearVelocity = (_targetPosition - GetUnit().GetEntityOwner().GlobalPosition).Normalized() * _moveToComponent.Speed;
        }
    }
}