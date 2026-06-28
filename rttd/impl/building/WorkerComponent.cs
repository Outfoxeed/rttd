using Godot;

namespace RTTD;

[GlobalClass]
public partial class WorkerComponent : EntityComponent
{
    [Export] public float WorkSpeed { get; private set; } = 1f;
}