using Godot;

namespace RTTD;

[GlobalClass]
public partial class PlayerResourcesManager : SingletonNode<PlayerResourcesManager>
{
    [Export] public ResourcesData Resources { get; private set; }
}