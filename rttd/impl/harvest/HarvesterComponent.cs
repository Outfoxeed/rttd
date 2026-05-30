using Godot;

namespace RTTD;

[GlobalClass]
public partial class HarvesterComponent : EntityComponent
{
    [Export] public float SpeedFactor = 1f;
}