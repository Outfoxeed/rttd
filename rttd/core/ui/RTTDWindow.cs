using System;
using Godot;

namespace RTTD;

[GlobalClass]
public partial class RTTDWindow : Control
{
    public Action<RTTDWindow> OnClosed;
    public Action<RTTDWindow, bool> OnFolded;
    
    [Export] private BaseButton _closeButton;
    [Export] private bool _foldable;
    [Export] private BaseButton _foldButton;
    [Export] private Control _contentContainer;

    private bool _folded = false;

    public override void _EnterTree()
    {
        base._EnterTree();

        if (_closeButton is not null) _closeButton.Pressed += OnCloseButtonPressed;
        if (_foldButton is not null)
        {
            _foldButton.SetVisible(_foldable);
            _foldButton.Pressed += OnFoldButtonPressed;
        }

        _folded = false;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        
        if (_closeButton is not null) _closeButton.Pressed -= OnCloseButtonPressed;
        if(_foldButton is not null) _foldButton.Pressed -= OnFoldButtonPressed;
    }
    
    public void Close()
    {
        SetVisible(false);
        QueueFree();
        
        OnClosed?.Invoke(this);
    }

    private void Fold()
    {
        _folded = !_folded;
        _contentContainer?.SetVisible(!_folded);
        
        OnFolded?.Invoke(this, _folded);
    }

    private void OnFoldButtonPressed()
    {
        Fold();
    }

    private void OnCloseButtonPressed()
    {
        Close();
    }
}