using System;
using System.Threading.Tasks;
using Godot;

namespace RTTD;

public abstract class MoveToCommandBase : UnitCommand
{
    private const ulong UpdateIntervalInMilliseconds = 200;
    protected const float SuccessDistanceThreshold = 32f;
    
    protected MoveToComponent _moveToComponent;
    private ulong _lastUpdateTime = 0;

    protected MoveToCommandBase(Action onSuccess = null, Action onFailed = null) : base(onSuccess, onFailed)
    {
    }

    protected override Task RunAsyncImpl()
    {
        _moveToComponent = GetUnit().GetEntityOwner().GetComponent<MoveToComponent>();
        
        UpdateVelocity();
        _lastUpdateTime = Time.GetTicksMsec();
        return Task.CompletedTask;
    }

    protected override Task CancelAsyncImpl()
    {
        GetUnit().GetEntityOwner().LinearVelocity = Vector2.Zero;
        return Task.CompletedTask;
    }

    protected override void ProcessImpl()
    {
        ulong time = Time.GetTicksMsec();
        if (_lastUpdateTime + UpdateIntervalInMilliseconds > time)
        {
            UpdateVelocity();
            _lastUpdateTime = time;
        }
    }

    protected abstract void UpdateVelocity();
}