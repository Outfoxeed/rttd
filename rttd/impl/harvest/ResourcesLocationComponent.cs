using Godot;

namespace RTTD;

[GlobalClass]
public partial class ResourcesLocationComponent : UnitOrderComponentVisitor
{
    [Export] public ResourceType ResourceType { get; private set; }
    [Export] public int Amount { get; private set; } = 100;

    [Export] public ulong HarvestIntervalInMilliseconds { get; private set; } = 1000;
    [Export] public int AmountPerHarvest { get; private set; } = 10;

    public void UpdateAmount(int amount)
    {
        Amount = Mathf.Max(0, amount);
    }

    public override bool CanVisit(UnitComponent target, OrderMode orderMode)
    {
        return Amount > 0 && target.GetEntity().TryGetComponent(out HarvesterComponent harvesterComponent) 
                          && harvesterComponent.CanHarvest(ResourceType);
    }

    protected override bool TryVisitImpl(UnitComponent target, OrderMode orderMode)
    {
        target.QueueCommand(new CompositeUnitCommand([
            new MoveToEntityCommand(GetEntity()),
            new HarvestCommand(this)
        ]), orderMode);
        return true;
    }
}