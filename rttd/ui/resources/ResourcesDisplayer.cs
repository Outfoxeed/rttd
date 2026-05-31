using Godot;

namespace RTTD;

[GlobalClass]
public partial class ResourcesDisplayer : Control
{
    public ResourceType ResourceType => _resourceType;
    
    [Export] private TextureRect _textureRect;
    [Export] private Label _label;
    
    private IResourcesData _resourcesData;
    private ResourceType _resourceType;
    
    public void Display(IResourcesData resourcesData, ResourceType resourceType)
    {
        _resourcesData = resourcesData;
        _resourceType = resourceType;

        _label.Text = _resourcesData.Resources[_resourceType].ToString();
        _textureRect.Texture = _resourceType.Texture;
        _textureRect.SelfModulate = _resourceType.ModuloColor;
    }

    public void OnResourcesAmountChanged(in ResourcesUpdatedArgs args)
    {
        if (args.Type != _resourceType)
            return;
        
        _label.Text = args.NewAmount.ToString();
        SetVisible(args.NewAmount > 0);
    }
}