using Godot;

namespace RTTD;

/// <summary>UnitComponent able to visit another Unit to apply an order on it</summary>
[GlobalClass]
public abstract partial class UnitOrderComponentVisitor : EntityComponent, IUnitOrderVisitor
{
    public abstract bool CanVisit(UnitComponent target, OrderMode orderMode);
    public bool TryVisit(UnitComponent target, OrderMode orderMode)
    {
        return CanVisit(target, orderMode) && TryVisitImpl(target, orderMode);
    }

    protected abstract bool TryVisitImpl(UnitComponent target, OrderMode orderMode);
}