namespace RTTD;

public interface IEntityComponentsContainer
{
    void AddComponent<T>(T component) where T : IEntityComponent;
    void RemoveComponent<T>(T component) where T : IEntityComponent;
    void RemoveAllComponents<T>() where T : IEntityComponent;

    bool HasComponent<T>() where T : IEntityComponent;
    bool TryGetComponent<T>(out T component) where T : IEntityComponent;
    T GetComponent<T>() where T : IEntityComponent;
    bool TryGetAllComponents<T>(out T[] components) where T : IEntityComponent;
    T[] GetAllComponents<T>() where T : IEntityComponent;
    IEntityComponent[] GetAllComponents();
}