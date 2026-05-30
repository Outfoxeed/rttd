using Godot;

namespace RTTD;

[GlobalClass]
public partial class ResourceType : Resource
{
    [Export] public string Name { get; private set; }
    [Export] public string Description { get; private set; }
    [Export] public Texture2D Texture { get; private set; }
    [Export] public Color ModuloColor { get; private set; }
}