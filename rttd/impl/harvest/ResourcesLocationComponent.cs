using Godot;

namespace RTTD;

[GlobalClass]
public partial class ResourcesLocationComponent : UnitVisitorComponent
{
    [Export] public ResourceType ResourceType { get; private set; }
    [Export] public int Amount { get; private set; } = 100;

    [Export] public ulong HarvestIntervalInMilliseconds { get; private set; } = 1000;
    [Export] public int AmountPerHarvest { get; private set; } = 10;

    public void UpdateAmount(int amount)
    {
        Amount = Mathf.Max(0, amount);
    }
    
    public override bool CanVisit(UnitComponent unitComponent)
    {
        return Amount > 0 && unitComponent.GetEntityOwner().HasComponent<HarvesterComponent>();
    }

    protected override bool TryVisitImpl(UnitComponent target)
    {
        target.ReplaceCurrentCommand(new CompositeUnitCommand([
            new MoveToEntityCommand(GetEntityOwner()),
            new HarvestCommand(this)
        ]));
        return true;
    }
}