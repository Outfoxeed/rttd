using Godot;

namespace RTTD;

public static class NodeExtensions
{
    public static bool IsValid<T>(this T node) where T : GodotObject
    {
        return GodotObject.IsInstanceValid(node) && !node.IsQueuedForDeletion();  
    }
}