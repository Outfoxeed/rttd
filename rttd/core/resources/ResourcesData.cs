using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.ExceptionServices;
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
    
    public void RemoveResources(ResourceType type, int amount)
    {
        if (!Resources.TryGetValue(type, out int currentAmount))
            return;
        
        int newAmount = Math.Max(0, currentAmount - amount);
        _resources[type] = newAmount;
        AmountChanged?.Invoke(new ResourcesUpdatedArgs(type, currentAmount, newAmount));
    }

    public void ClearResources(ResourceType type)
    {
        if (!_resources.TryGetValue(type, out int currentAmount))
            return;
        
        _resources[type] = 0;
        AmountChanged?.Invoke(new ResourcesUpdatedArgs(type, currentAmount, 0));
        AddResources(type, -_resources[type]);
    }
    
    public bool TryBuy(ResourceType type, int amount)
    {
        bool success = this.HasResources(type, amount);
        if(success) RemoveResources(type, amount);
        return success;
    }

    public bool TryBuy(IResourcesData cost)
    {
        if (!this.HasResources(cost))
            return false;

        foreach (var (resourceType, amount) in cost.Resources)
        {
            RemoveResources(resourceType, amount);
        }
        return true;
    }
}