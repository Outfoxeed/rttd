namespace RTTD;

/// <summary>Can visit a Unit to apply an order on it</summary>
public interface IUnitOrderVisitor
{
    bool CanVisitImpl(UnitComponent target, OrderMode orderMode);
    bool TryVisit(UnitComponent target, OrderMode orderMode);
}