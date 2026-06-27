using System.Collections.Generic;

namespace RTTD;

public class UnitCommandHandler : IUnitCommandHandler
{
    private readonly UnitComponent _unit;
    private readonly List<IUnitCommand> _commands = new();

    public UnitCommandHandler(UnitComponent unit)
    {
        _unit = unit;
    }

    public void Process()
    {
        if (TryGetCurrentCommand(out IUnitCommand currentCommand))
        {
            switch (currentCommand.GetState())
            {
                case UnitCommandState.Default:
                    currentCommand.RunAsync();
                    break;
                case UnitCommandState.Running:
                    currentCommand.Process();
                    break;
                case UnitCommandState.Success:
                case UnitCommandState.Failed:
                case UnitCommandState.Cancelled:
                    _commands.RemoveAt(0);
                    break;
                default:
                    break;
            }
        }
    }

    public void QueueCommand(IUnitCommand command, OrderMode orderMode)
    {
        if (orderMode is OrderMode.Single) ClearCommands();
        AddCommand(command);
    }

    public void ClearCommands()
    {
        foreach (IUnitCommand command in _commands)
        {
            _ = command.CancelAsync();
        }
    }

    public bool HasCommandInProgress()
    {
        return TryGetCurrentCommand(out IUnitCommand currentCommand) && currentCommand.GetState() != UnitCommandState.Default;
    }
    
    private void AddCommand(IUnitCommand command)
    {
        command.SetUnit(_unit);
        _commands.Add(command);
    }
    
    private bool TryGetCurrentCommand(out IUnitCommand command)
    {
        if (_commands.Count == 0)
        {
            command = null;
            return false;
        }
        
        command = _commands[0];
        return true;
    }
}