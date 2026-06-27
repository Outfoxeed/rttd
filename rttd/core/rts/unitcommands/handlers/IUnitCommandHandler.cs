namespace RTTD;

public interface IUnitCommandHandler
{
    void QueueCommand(IUnitCommand command, OrderMode orderMode);
    void ClearCommands();

    bool HasCommandInProgress();
}