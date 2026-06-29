using System.Collections.Generic;
using Godot;

namespace RTTD;

[GlobalClass]
public partial class UIManager : SingletonNode<UIManager>
{
    [Export] private Control _windowsContainer;
    
    private readonly Dictionary<object, RTTDWindowBase> _windows = new();
    
    public override void _ExitTree()
    {
        base._ExitTree();

        foreach (RTTDWindowBase window in _windows.Values)
        {
            if (window.IsValid())
                window.OnClosed -= OnWindowClosed;
        }
    }

    public void AddWindow(object owner, PackedScene windowPackedScene, Vector2 screenPosition)
    {
        if (_windows.ContainsKey(owner))
            return;
        
        RTTDWindowBase newWindow = windowPackedScene.Instantiate<RTTDWindowBase>();
        newWindow.SetOwner(owner);
        _windowsContainer.AddChild(newWindow);
        newWindow.Position = screenPosition;
        _windows.Add(owner, newWindow);
        newWindow.OnClosed += OnWindowClosed;
    }

    public void CloseAllWindows()
    {
        foreach (RTTDWindowBase window in _windows.Values)
        {
            window.Close();
        }
    }
    
    private void OnWindowClosed(RTTDWindowBase closedWindow)
    {
        closedWindow.OnClosed -= OnWindowClosed;
        
        foreach (var (owner, window) in _windows)
        {
            if (window == closedWindow)
            {
                _windows.Remove(owner);
                break;
            }
        }
    }
}