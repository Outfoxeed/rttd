using System.Collections.Generic;
using Godot;

namespace RTTD;

public partial class RTSOrderSystem
{
    private readonly List<UnitComponent> _unitsWaitingForOrder = new();
    private void ApplyOrder(IReadOnlyList<UnitComponent> units, Vector2 targetPosition, Entity targetEntity = null)
    {
        GD.Print($"Apply order: {units.Count} unit(s) for targetEntity '{targetEntity?.GetName() ?? "NULL"}' and targetPosition {targetPosition}");
        if (units.Count == 0)
            return;
        
        // Give a MoveToPosition to all units in the case where only a location is targeted 
        if (targetEntity == null)
        {
            foreach (UnitComponent unit in units)
            {
                if (unit.GetEntityOwner().TryGetComponent(out MoveToComponent moveToComponent))
                {
                    Vector2 unitTargetPosition = targetPosition + Random.GetRandomPointInCircle(units.Count * 16);
                    unit.ReplaceCurrentCommand(new MoveToPositionCommand(unitTargetPosition));
                }
            }

            return;
        }
        
        _unitsWaitingForOrder.Clear();
        foreach (var unit in units) _unitsWaitingForOrder.Add(unit);
        // if the targetEntity is valid, we iterate through all its UnitVisitorComponent and try to visit the "idle" units (the UnitVisitor will add commands onto the Unit if possible)
        // if a Unit has been successfully visited by a UnitVisitorComponent, the Unit gets removed from the list (we consider the unit already received an order from the visitor)
        foreach (Node child in targetEntity.GetChildren(false))
        {
            if (child is not UnitVisitorComponent unitVisitor)
                continue;

            for (int i = 0; i < _unitsWaitingForOrder.Count; i++)
            {
                if (unitVisitor.TryVisit(_unitsWaitingForOrder[i]))
                {
                    _unitsWaitingForOrder.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}