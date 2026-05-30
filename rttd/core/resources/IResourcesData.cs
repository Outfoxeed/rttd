using System;
using System.Collections.Generic;

namespace RTTD;

public interface IResourcesData
{
    IReadOnlyDictionary<ResourceType, int> Resources { get; }
    event Action<ResourcesUpdatedArgs> AmountChanged;
}

public readonly struct ResourcesUpdatedArgs(ResourceType type, int oldAmount, int newAmount);
