using System.Collections.Generic;
using Godot;

namespace RTTD;

[GlobalClass]
public partial class UnitComponent : EntityComponent, IUnitCommandHandler
{
    public static IReadOnlyList<UnitComponent> AllUnits => _allUnits;
    private static readonly List<UnitComponent> _allUnits = new();
    
    private readonly UnitCommandHandler _commandHandler;

    public UnitComponent()
    {
        _commandHandler = new UnitCommandHandler(this);
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        _allUnits.Add(this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _allUnits.Remove(this);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        _commandHandler.Process();
    }

    public void AddCommand(IUnitCommand command) => _commandHandler.AddCommand(command);
    public void ReplaceCurrentCommand(IUnitCommand command) { _commandHandler.ReplaceCurrentCommand(command); }
    public void ClearCommands() => _commandHandler.ClearCommands();
    public bool HasCommandInProgress() => _commandHandler.HasCommandInProgress();
}