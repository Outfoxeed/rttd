using System;
using System.Collections.Generic;

namespace RTTD;

public partial class EntityComponentsContainer : IEntityComponentsContainer
{
    private readonly Entity _entity;
    private readonly List<ComponentEntry> _entries = new();

    public EntityComponentsContainer(Entity entity)
    {
        _entity = entity;

        int childCount = entity.GetChildCount();
        for (int i = 0; i < childCount; i++)
        {
            if (entity.GetChild(i) is IEntityComponent component)
            {
                AddComponent(component);
            }
        }
    }

    public void AddComponent<T>(T component) where T : IEntityComponent
    {
        if (!TryGetComponentEntry<T>(out int componentEntryIndex))
        {
            _entries.Add(new ComponentEntry(_entity, component));
            return;
        }
        
        _entries[componentEntryIndex].Add(component);
    }

    private void AddComponent(IEntityComponent component)
    {
        if (!TryGetComponentEntry(component.GetType(), out int componentEntryIndex))
        {
            _entries.Add(new ComponentEntry(_entity, component));
            return;
        }
        
        _entries[componentEntryIndex].Add(component);
    }

    public void RemoveComponent<T>(T component) where T : IEntityComponent
    {
        if (!TryGetComponentEntry<T>(out int componentEntryIndex)) 
            return;
        
        _entries[componentEntryIndex].Remove(component);
        if (_entries[componentEntryIndex].IsEmpty())
        {
            _entries.RemoveAt(componentEntryIndex);
        }
    }

    public void RemoveAllComponents<T>() where T : IEntityComponent
    {
        if (!TryGetComponentEntry<T>(out int componentEntryIndex))
            return;
        
        _entries[componentEntryIndex].RemoveAll();
        _entries.RemoveAt(componentEntryIndex);
    }

    public bool HasComponent<T>() where T : IEntityComponent
    {
        return TryGetComponentEntry<T>(out int _);
    }

    public bool TryGetComponent<T>(out T component) where T : IEntityComponent
    {
        if (!TryGetComponentEntry<T>(out int componentEntryIndex))
        {
            component = default;
            return false;
        }

        component = _entries[componentEntryIndex].Get<T>();
        return true;
    }

    public T GetComponent<T>() where T : IEntityComponent
    {
        if (!TryGetComponentEntry<T>(out int componentEntryIndex))
            return default;
        
        return _entries[componentEntryIndex].Get<T>();
    }

    public bool TryGetAllComponents<T>(out T[] components) where T : IEntityComponent
    {
        if (!TryGetComponentEntry<T>(out int componentEntryIndex))
        {
            components = Array.Empty<T>();
            return false;
        }

        components = _entries[componentEntryIndex].GetAll<T>();
        return true;
    }

    public T[] GetAllComponents<T>() where T : IEntityComponent
    {
        if (!TryGetComponentEntry<T>(out int componentEntryIndex))
            return Array.Empty<T>();
        
        return _entries[componentEntryIndex].GetAll<T>();
    }
    
    private bool TryGetComponentEntry<T>(out int componentEntryIndex) => TryGetComponentEntry(typeof(T), out componentEntryIndex);
    private bool TryGetComponentEntry(Type componentType, out int componentEntryIndex)
    {
        for (int i = 0; i < _entries.Count; i++)
        {
            if (_entries[i].ComponentType == componentType)
            {
                componentEntryIndex = i;
                return true;
            }
        }

        componentEntryIndex = -1;
        return false;
    }
}