using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace RTTD;

[GlobalClass]
public partial class ResourcesDataDisplayer : Control
{
    [Export] private ResourcesData _resources;
    [Export] private PackedScene _resourcesDisplayerPackedScene;
    [Export] private Control _resourcesDisplayerContainer;

    private readonly List<ResourcesDisplayer> _resourcesDisplayers = new();

    public override void _EnterTree()
    {
        base._EnterTree();

        foreach (ResourceType resourceType in _resources.Resources.Keys)
        {
            AddResourcesDisplayer(resourceType);
        }
        
        _resources.AmountChanged += OnResourcesAmountChanged;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (_resources != null)
        {
            _resources.AmountChanged -= OnResourcesAmountChanged;
        }

        Array<Node> children = _resourcesDisplayerContainer.GetChildren();
        foreach (Node child in children)
        {
            child.QueueFree();
        }
        _resourcesDisplayers.Clear();
    }
    
    private void OnResourcesAmountChanged(ResourcesUpdatedArgs args)
    {
        foreach (ResourcesDisplayer resourcesDisplayer in _resourcesDisplayers)
        {
            if (resourcesDisplayer.ResourceType == args.Type)
            {
                resourcesDisplayer.OnResourcesAmountChanged(args);
                return;
            }
        }
        
        AddResourcesDisplayer(args.Type);
    }

    private void AddResourcesDisplayer(ResourceType resourceType)
    {
        ResourcesDisplayer resourcesDisplayer = _resourcesDisplayerPackedScene.Instantiate<ResourcesDisplayer>();
        _resourcesDisplayerContainer.AddChild(resourcesDisplayer);
        _resourcesDisplayers.Add(resourcesDisplayer);
        resourcesDisplayer.Display(_resources, resourceType);
    }
}