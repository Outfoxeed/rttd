namespace RTTD;

public static class ResourcesDataExtensions
{
    public static bool HasResources(this IResourcesData resourcesData, ResourceType type, int amount) => resourcesData.Resources.TryGetValue(type, out int currentAmount) && currentAmount >= amount;
    public static bool HasResources(this IResourcesData resourcesData, IResourcesData other)
    {
        foreach (var (resourceType, amount) in other.Resources)
        {
            if (!resourcesData.HasResources(resourceType, amount))
                return false;
        }

        return true;
    }
}