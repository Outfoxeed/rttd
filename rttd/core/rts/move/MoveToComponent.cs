using Godot;

namespace RTTD;

[GlobalClass]
public partial class MoveToComponent : EntityComponent
{
    [Export] public float Speed { get; set; } = 200f;
}