namespace RTTD;

public interface IUnitCommandHandler
{
    void AddCommand(IUnitCommand command);
    void ReplaceCurrentCommand(IUnitCommand command);
    void ClearCommands();

    bool HasCommandInProgress();
}