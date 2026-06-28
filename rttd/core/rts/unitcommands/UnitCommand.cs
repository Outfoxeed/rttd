using System;
using System.Threading.Tasks;

namespace RTTD;

public abstract class UnitCommand : IUnitCommand
{
    private UnitComponent _unit;
    private Action _onFailedCallback;
    private Action _onSuccessCallback;
    private UnitCommandState _state = UnitCommandState.Default;

    protected UnitCommand(Action onSuccess = null, Action onFailed = null)
    {
        _onSuccessCallback = onSuccess;
        _onFailedCallback = onFailed;
    }
    
    public async Task RunAsync()
    {
        if (_unit == null)
        {
            Logger.LogError(null, $"Command {this} has no valid unit! Cannot start command.");
            SetState(UnitCommandState.Failed);
            return;
        }
        
        if (GetState() != UnitCommandState.Default)
        {
            Logger.LogError(_unit, $"Command {this} already ran ({_state}). Cannot run it again.");
            SetState(UnitCommandState.Failed);
            return;
        }
        
        SetState(UnitCommandState.Starting);
        try
        {
            bool runAsyncSuccess = await RunAsyncImpl();
            SetState(runAsyncSuccess ? UnitCommandState.Running : UnitCommandState.Failed);
        }
        catch (Exception e)
        {
            Logger.LogError(_unit, $"Command: caught exception {e.Message} during StartAsyncImpl of {this}\n{e.StackTrace}");
            SetState(UnitCommandState.Failed);
        }
    }
    protected abstract Task<bool> RunAsyncImpl(); // Throwing an exception in the impl results in a failed command

    public async Task CancelAsync()
    {
        if (GetState() is UnitCommandState.Default)
        {
            SetState(UnitCommandState.Cancelled);
            return;
        }
        
        if (GetState() is UnitCommandState.Cancelling or UnitCommandState.Cancelled)
        {
            Logger.LogError(_unit, $"Command {this} already cancelling or cancelled. Will ignore the cancel request.");
            return;
        }

        SetState(UnitCommandState.Cancelling);
        try
        {
            await CancelAsyncImpl();
        }
        catch (Exception e)
        {
            Logger.LogError(_unit, $"Command: caught exception {e.Message} during StopAsyncImpl of {this}\n{e.StackTrace}");
            SetState(UnitCommandState.Failed);
        }
        SetState(UnitCommandState.Cancelled);
    }
    protected abstract Task CancelAsyncImpl();

    public void Process()
    {
        if (GetState() != UnitCommandState.Running)
            return;
           
        ProcessImpl();
    }
    protected abstract void ProcessImpl();

    protected Entity GetEntity() => GetUnit().GetEntity();
    public UnitComponent GetUnit() => _unit;
    public void SetUnit(UnitComponent unit) => _unit = unit;
    public UnitCommandState GetState() => _state;
    protected void SetState(UnitCommandState state)
    {
        if (_state == state)
            return;
        _state = state;
        
        if (state is UnitCommandState.Failed)
        {
            _onFailedCallback?.Invoke();
        }
        else if (state is UnitCommandState.Success)
        {
            _onSuccessCallback?.Invoke();
        }
    }

    public override string ToString()
    {
        return $"{GetType().Name}({_unit.GetEntity()}|{GetState()})";
    }
}
