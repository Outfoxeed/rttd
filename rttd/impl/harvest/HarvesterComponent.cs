using Godot;

namespace RTTD;

[GlobalClass]
public partial class HarvesterComponent : EntityComponent
{
    [Export] private Godot.Collections.Dictionary<ResourceType, float> _speedFactors = new();
    
    public bool CanHarvest(ResourceType resourceType) => _speedFactors.ContainsKey(resourceType);
    public float GetSpeedFactor(ResourceType resourceType)
    {
        if (_speedFactors.TryGetValue(resourceType, out float speedFactor))
            return speedFactor;
        return 0f;
    }
}