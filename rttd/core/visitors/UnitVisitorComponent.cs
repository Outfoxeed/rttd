using Godot;

namespace RTTD;

[GlobalClass]
public abstract partial class UnitVisitorComponent : EntityComponent, IVisitor<UnitComponent>
{
    public abstract bool CanVisit(UnitComponent unitComponent);
    public bool TryVisit(UnitComponent target)
    {
        return CanVisit(target) && TryVisitImpl(target);
    }

    protected abstract bool TryVisitImpl(UnitComponent target);
}