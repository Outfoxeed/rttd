namespace RTTD;

/// <summary>Can visit a Unit to apply an order on it</summary>
public interface IUnitOrderVisitor
{
    bool CanVisit(UnitComponent target, OrderMode orderMode);
    bool TryVisit(UnitComponent target, OrderMode orderMode);
}