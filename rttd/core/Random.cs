using Godot;

namespace RTTD;

public static class Random
{
    private static RandomNumberGenerator _rng = new();
    private const float MaxRadianAngle = 4f * Mathf.Pi;

    public static uint GetRandomUInt() => _rng.Randi();
    public static float GetRandomFloat() => _rng.Randf();
    public static float GetRandomFloat(float min, float max) => _rng.RandfRange(min, max);
    public static float GetRandomRadianAngle() => GetRandomFloat(0, MaxRadianAngle);
    public static float GetRandomDegreeAngle() => GetRandomFloat(0, 360f);

    public static Vector2 GetRandomPointInCircle(float radius)
    {
        float angle = GetRandomRadianAngle();
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * (radius * _rng.RandfRange(0.4f, 1f));
    }

    public static Vector2 GetRandomPointAlongCircle(float radius)
    {
        float angle = GetRandomRadianAngle();
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
    }
}