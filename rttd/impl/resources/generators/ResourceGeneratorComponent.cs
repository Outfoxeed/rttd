using Godot;

namespace RTTD;

[GlobalClass]
public partial class ResourceGeneratorComponent : EntityComponent
{
    [Export] private ResourceType _resourceType;
    [Export] private int _resourceGainAmount = 10;
    [Export] private float _resourceGainAmountIntervalInSeconds = 2f;

    private Timer _timer;
    
    public override void _EnterTree()
    {
        base._EnterTree();

        if (_timer == null)
        {
            _timer = new Timer();
            AddChild(_timer);
        }
        _timer.Timeout += OnTimerTimeout;
        _timer.Start(_resourceGainAmountIntervalInSeconds);
    }

    private void OnTimerTimeout()
    {
        PlayerResourcesManager.Instance.Resources.AddResources(_resourceType, _resourceGainAmount);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (_timer != null)
        {
            _timer.Timeout -= OnTimerTimeout;
            _timer.Stop();
        }
    }
}