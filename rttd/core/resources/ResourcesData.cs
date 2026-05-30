using System;
using System.Collections.Generic;
using Godot;

namespace RTTD;

[GlobalClass]
public partial class ResourcesData : Resource, IResourcesData
{
    public IReadOnlyDictionary<ResourceType, int> Resources => _resources;
    
    public event Action<ResourcesUpdatedArgs> AmountChanged;
    
    [Export] private Godot.Collections.Dictionary<ResourceType, int> _resources = new();

    public void AddResources(ResourceType type, int amount)
    {
        int oldAmount = 0;
        if (!_resources.TryAdd(type, amount))
        {
            oldAmount = _resources[type];
            _resources[type] += amount;
        }

        GD.Print($"Resources: added {_resources[type] - oldAmount} {type.Name} to {ResourceName}. Now {_resources[type]} of {type.Name}!");
        AmountChanged?.Invoke(new ResourcesUpdatedArgs(type, oldAmount, _resources[type]));
    }
    
    public void RemoveResources(ResourceType type, int amount) => AddResources(type, -amount);
    public void ClearResources(ResourceType type)
    {
        if (!_resources.ContainsKey(type))
            return;
        AddResources(type, -_resources[type]);
    }
}