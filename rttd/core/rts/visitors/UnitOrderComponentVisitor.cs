using Godot;

namespace RTTD;

/// <summary>UnitComponent able to visit another Unit to apply an order on it</summary>
[GlobalClass]
public abstract partial class UnitOrderComponentVisitor : EntityComponent, IUnitOrderVisitor
{
    public bool CanVisit(UnitComponent target, OrderMode orderMode)
    {
        return Enabled && CanVisitImpl(target, orderMode);
    }
    public bool TryVisit(UnitComponent target, OrderMode orderMode)
    {
        return CanVisit(target, orderMode) && TryVisitImpl(target, orderMode);
    }

    public abstract bool CanVisitImpl(UnitComponent target, OrderMode orderMode);
    protected abstract bool TryVisitImpl(UnitComponent target, OrderMode orderMode);
}