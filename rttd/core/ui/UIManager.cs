using System.Collections.Generic;
using Godot;

namespace RTTD;

[GlobalClass]
public partial class UIManager : SingletonNode<UIManager>
{
    [Export] private Control _windowsContainer;
    
    private readonly Dictionary<object, RTTDWindow> _windows = new();
    
    public override void _ExitTree()
    {
        base._ExitTree();

        foreach (RTTDWindow window in _windows.Values)
        {
            if (window != null)
                window.OnClosed -= OnWindowClosed;
        }
    }

    public void AddWindow(object owner, PackedScene windowPackedScene, Vector2 screenPosition)
    {
        if (_windows.ContainsKey(owner))
            return;
        
        RTTDWindow newWindow = windowPackedScene.Instantiate<RTTDWindow>();
        _windowsContainer.AddChild(newWindow);
        newWindow.Position = screenPosition;
        _windows.Add(owner, newWindow);
        newWindow.OnClosed += OnWindowClosed;
    }

    public void CloseAllWindows()
    {
        foreach (RTTDWindow window in _windows.Values)
        {
            window.Close();
        }
    }
    
    private void OnWindowClosed(RTTDWindow closedWindow)
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