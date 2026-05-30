using System.Threading.Tasks;

namespace RTTD;

public interface IUnitCommand
{
    UnitComponent GetUnit();
    void SetUnit(UnitComponent unit);
    
    UnitCommandState GetState();

    Task RunAsync();
    Task CancelAsync();
    void Process();
}