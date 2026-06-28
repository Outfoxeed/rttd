using System;
using System.Collections.Generic;
using Godot;

namespace RTTD;

public partial class EntityComponentsContainer
{
    private struct ComponentEntry
    {
        public Type ComponentType { get; }
        public IReadOnlyCollection<IEntityComponent> Components => _components;
        private IEntityComponent[] _components;
        private readonly Entity _entity;

        public ComponentEntry(Entity entity, IEntityComponent component)
        {
            _entity = entity;
            ComponentType = component.GetType();
            _components = [component];
        }

        public ComponentEntry(Entity entity, IEntityComponent[] components)
        {
            _entity = entity;
            ComponentType = components[0].GetType();
            _components = components;
        }

        public void Add(IEntityComponent component)
        {
            if (component == null)
            {
                Logger.LogError(this, $"Trying to add a null component into {this}");
                return;
            }
            
            if (component.GetType() != ComponentType)
            {
                Logger.LogError(this, $"Trying to add a component of type '{component.GetType()}' into {this}");
                return;
            }
            
            IEntityComponent[] newComponents = new IEntityComponent[_components.Length + 1];
            _components.CopyTo(newComponents, 0);
            newComponents[^1] = component;
            _components = newComponents;

            if (component is Node nodeComponent)
            {
                _entity.AddChild(nodeComponent);
            }
        }

        public void Remove(IEntityComponent component)
        {
            if (component == null)
            {
                Logger.LogError(this, $"Trying to remove a null component from {this}");
                return;
            }

            if (!TryGetComponentIndex(component, out int componentIndex))
            {
                Logger.LogError(this, $"Trying to remove a component that is not already contained in {this}");
                return;
            }

            IEntityComponent[] newComponents = new IEntityComponent[_components.Length - 1];
            int index = 0;
            for (int j = 0; j < _components.Length; j++)
            {
                if (j == componentIndex)
                    continue;
                newComponents[index] = _components[j];
                index++;
            }
            _components = newComponents;

            if (component is Node nodeComponent)
            {
                _entity.RemoveChild(nodeComponent);
            }
        }

        public void RemoveAll()
        {
            foreach (var component in _components)
            {
                if (component is Node nodeComponent)
                {
                    _entity.RemoveChild(nodeComponent);
                }
            }

            _components = null;
        }

        public T Get<T>() where T : IEntityComponent
        {
            if (ComponentType != typeof(T))
            {
                Logger.LogError(this, $"Trying to get a component of type '{typeof(T)}' from {this}");
                return default;
            }
            
            return (T)_components[0];
        }

        public T[] GetAll<T>() where T : IEntityComponent
        {
            if (ComponentType != typeof(T))
            {
                Logger.LogError(this, $"Trying to get a component of type '{typeof(T)}' from {this}");
                return Array.Empty<T>();
            }
            
            T[] components = new T[_components.Length];
            _components.CopyTo(components, 0);
            return components;
        }

        public bool IsEmpty()
        {
            return _components == null || _components.Length == 0;
        }
        
        public override string ToString()
        {
            return $"{GetType().Name}<{ComponentType.Name}>({_components.Length})";
        }

        private bool TryGetComponentIndex(IEntityComponent component, out int componentIndex)
        {
            for (var i = 0; i < _components.Length; i++)
            {
                if (_components[i] == component)
                {
                    componentIndex = i;
                    return true;
                }
            }

            componentIndex = -1;
            return false;
        }
    }
}