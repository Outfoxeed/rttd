using Godot;

namespace RTTD;

public static class Logger
{
    private static readonly string _prefix = "[RTTD]";
    public static void LogError(GodotObject @object, string message) => GD.PrintErr($"{_prefix}[{@object?.GetType().Name}] {message}");
}