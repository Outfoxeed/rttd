using System.Threading.Tasks;
using Godot;

namespace RTTD;

public class HarvestCommand : UnitCommand
{
    private ResourcesLocationComponent _location;
    private HarvesterComponent _harvester;

    private ulong _lastHarvestTime;

    public HarvestCommand(ResourcesLocationComponent location)
    {
        _location = location;
    }

    protected override Task RunAsyncImpl()
    {
        _harvester = GetEntity().GetComponent<HarvesterComponent>();
        _lastHarvestTime = Time.GetTicksMsec();
        GetEntity().LinearVelocity = Vector2.Zero;
        
        return Task.CompletedTask;
    }

    protected override Task CancelAsyncImpl()
    {
        return Task.CompletedTask;
    }

    protected override void ProcessImpl()
    {
        if (_location == null || _location.Amount <= 0)
        {
            SetState(UnitCommandState.Success);
            return;
        }
        
        ulong time = Time.GetTicksMsec();
        ulong nextHarvestTime = _lastHarvestTime + (ulong)(_location.HarvestIntervalInMilliseconds / _harvester.SpeedFactor);
        if (time >= nextHarvestTime)
        {
            PlayerResourcesManager.Instance.Resources.AddResources(_location.ResourceType, Mathf.Min(_location.AmountPerHarvest, _location.Amount));
            _location.UpdateAmount(_location.Amount - _location.AmountPerHarvest);
            _lastHarvestTime = time;
        }
    }
}