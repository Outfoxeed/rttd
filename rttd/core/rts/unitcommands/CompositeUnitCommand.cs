using System.Threading.Tasks;

namespace RTTD;

public class CompositeUnitCommand : UnitCommand
{
    private readonly IUnitCommand[] _commands;
    private int _currentCommandIndex = 0;

    public CompositeUnitCommand(IUnitCommand[] commands)
    {
        _commands = commands;
    }

    protected override async Task<bool> RunAsyncImpl()
    {
        if (_commands.Length == 0)
        {
            SetState(UnitCommandState.Failed);
            return false;
        }
        
        foreach (IUnitCommand command in _commands)
        {
            command.SetUnit(GetUnit());
        }

        IUnitCommand firstCommand = _commands[0];
        await firstCommand.RunAsync();
        return firstCommand.GetState() is not UnitCommandState.Failed;
    }

    protected override async Task CancelAsyncImpl()
    {
        Task[] cancelTasks = new Task[_commands.Length - _currentCommandIndex];
        for (int i = _currentCommandIndex; i < _commands.Length; i++)
        {
            cancelTasks[i - _currentCommandIndex] = _commands[i].CancelAsync();
        }
        await Task.WhenAll(cancelTasks);
    }

    protected override void ProcessImpl()
    {
        IUnitCommand currentCommand = _commands[_currentCommandIndex];
        if (currentCommand == null)
        {
            SetState(UnitCommandState.Failed);
            return;
        }

        switch (currentCommand.GetState())
        {
            case UnitCommandState.Running:
                currentCommand.Process();
                break;
            case UnitCommandState.Success:
                _currentCommandIndex++;
                if (_currentCommandIndex >= _commands.Length)
                {
                    SetState(UnitCommandState.Success);
                }
                else
                {
                    _ = _commands[_currentCommandIndex].RunAsync();
                }
                break;
            case UnitCommandState.Failed:
                SetState(UnitCommandState.Failed);
                break;
        }
    }
}