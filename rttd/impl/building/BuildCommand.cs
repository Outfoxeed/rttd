using System.Threading.Tasks;
using Godot;

namespace RTTD;

public class BuildCommand : UnitCommand
{
    private readonly BuildingComponent _buildingComponent;
    private WorkerComponent _workerComponent;
    private Timer _timer;
    private readonly float TimerIntervalInSeconds = 1f;

    public BuildCommand(BuildingComponent buildingComponent)
    {
        _buildingComponent = buildingComponent;
    }

    protected override Task<bool> RunAsyncImpl()
    {
        if (_buildingComponent.CurrentState is BuildingComponent.State.MissingResources)
        {
            if (!_buildingComponent.TryGetMissingResources())
            {
                return Task.FromResult(false);
            }
        }
        
        _workerComponent = GetEntity().GetComponent<WorkerComponent>();
        
        _timer = new Timer();
        _workerComponent.AddChild(_timer);
        _timer.Timeout += OnTimerTimeout;
        _timer.Start(TimerIntervalInSeconds);
        
        GetEntity().LinearVelocity = Vector2.Zero;
        
        return Task.FromResult(true);
    }

    protected override Task CancelAsyncImpl()
    {
        return Task.CompletedTask;
    }

    protected override void ProcessImpl()
    {
        if (!_buildingComponent.IsValid())
        {
            SetState(UnitCommandState.Failed);
            return;
        }
        
        if(_buildingComponent.CurrentState is BuildingComponent.State.Constructed)
        {
            SetState(UnitCommandState.Success);
            return;
        }
    }

    protected override void OnFinishedImpl()
    {
        base.OnFinishedImpl();

        if (_timer.IsValid())
        {
            _timer.Timeout -= OnTimerTimeout;
            _timer.Stop();

            if (_workerComponent.IsValid())
            {
                _workerComponent.RemoveChild(_timer);
            }
        }
    }

    private void OnTimerTimeout()
    {
        if (!_buildingComponent.IsValid())
        {
            SetState(UnitCommandState.Failed);
            return;
        }

        bool constructionFinished = _buildingComponent.AddConstructionProgress(_workerComponent.WorkSpeed * TimerIntervalInSeconds);
        if (constructionFinished)
        {
            SetState(UnitCommandState.Success);
        }
    }
    
}