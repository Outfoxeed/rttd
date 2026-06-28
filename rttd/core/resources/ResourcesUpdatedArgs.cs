namespace RTTD;

public readonly struct ResourcesUpdatedArgs(ResourceType type, int oldAmount, int newAmount)
{
    public ResourceType Type { get; } = type;
    public int OldAmount { get; } = oldAmount;
    public int NewAmount { get; } = newAmount;
}